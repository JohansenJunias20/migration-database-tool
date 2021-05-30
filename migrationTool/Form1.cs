using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using FirebirdSql.Data.FirebirdClient;
using System.Globalization;
using System.Threading;

namespace migrationTool
{
    public partial class Form1 : Form
    {
        dynamic config;
        public Form1()
        {
            InitializeComponent();
            toolTip2.SetToolTip(ignoreExistCheckBox, "ignore warning table already exist. If your mysql database target already has tables you can check this checkbox");
            toolTipDuplicateKey.SetToolTip(ignoreDuplicateKey, "ignore warning data/row already exist. If your mysql database target already has several same data with firebird source database you can check this checkbox");
            toolTipDuplicateKey.SetToolTip(forceModeCheckBox, "ignore all exception");
            var config = System.IO.File.ReadAllText("./config.json");
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(config);
            this.config = result;
            pathSource.Text = result["source"]["path"];
            usernameSource.Text = result["source"]["username"];
            passwordSource.Text = result["source"]["password"];
            ipTarget.Text = result["target"]["ip"];
            portTarget.Text = result["target"]["port"];
            usernameTarget.Text = result["target"]["username"];
            databaseTarget.Text = result["target"]["database"];
            passwordTarget.Text = result["target"]["password"];
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        MySqlConnection connMysql;

        FbConnectionStringBuilder csb;
        FbConnection connFB;
        FbCommand cmd;
        FbDataReader readerFB;
        float progress = 0f;
        string Log = "";
        void openConnection()
        {
            changeLog("Opening NYSQL Connection");

            connMysql = new MySqlConnection(
                $"Server={ipTarget.Text};user={usernameTarget.Text};database={databaseTarget.Text};password={passwordTarget.Text}"
                );
            try
            {
                connMysql.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Open Connection MYSQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
            changeLog("sucess Open MYSQL Connection");

            backgroundWorker1.ReportProgress((int)(progress += 3));
            csb = new FbConnectionStringBuilder();
            csb.Database = pathSource.Text;
            csb.DataSource = "localhost";
            csb.Port = 3050;
            csb.UserID = usernameSource.Text;
            csb.Password = passwordSource.Text;
            connFB = new FbConnection(csb.ToString());
            changeLog("Opening Open Firebird Connection");
            try
            {
                connFB.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Open Connection Firebird", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);

            }
            changeLog("sucess Open MYSQL Connection");

            backgroundWorker1.ReportProgress((int)(progress += 3));



        }
        List<string> getAllTableList()
        {
            List<string> tableList = new List<string>();
            changeLog("preparing table list query");

            cmd = new FbCommand(
             "SELECT a.RDB$RELATION_NAME " +
             "FROM RDB$RELATIONS a " +
             "WHERE COALESCE(RDB$SYSTEM_FLAG, 0) = 0 AND RDB$RELATION_TYPE = 0"
             , connFB);

            readerFB = cmd.ExecuteReader();
            changeLog("executed reader");

            while (readerFB.Read())
            {
                changeLog($"read Tabel List Firebird: {readerFB.GetString(0).Replace(" ", "")}");

                tableList.Add(readerFB.GetString(0).Replace(" ", ""));
            }
            changeLog("Done fetching table names");

            return tableList;


        }
        List<string> getPrimaryKeys(string tableName)
        {
            string sql =
                $"select " +
                " " +
                "sg.rdb$field_name as field_name, " +
                "rc.rdb$relation_name as table_name " +
                "from " +
                "rdb$indices ix " +
                "left join rdb$index_segments sg on ix.rdb$index_name = sg.rdb$index_name " +
                "left join rdb$relation_constraints rc on rc.rdb$index_name = ix.rdb$index_name " +
                "where " +
                $"rc.rdb$constraint_type = 'PRIMARY KEY' AND rc.rdb$relation_name  = '{tableName}'"
                ;
            FbCommand query = new FbCommand(sql, connFB);

            FbDataReader reader = query.ExecuteReader();
            List<string> result = new List<string>();
            while (reader.Read())
            {
                result.Add(reader.GetString(0).Replace(" ", ""));
            }
            return result;
        }
        List<string> columnDesc(string tableName)
        {

            string sql =
                "SELECT "
+ "  RF.RDB$FIELD_NAME FIELD_NAME, "
+ "  CASE F.RDB$FIELD_TYPE "
+ "    WHEN 7 THEN "
+ "      CASE F.RDB$FIELD_SUB_TYPE "
+ "        WHEN 0 THEN 'SMALLINT' "
+ "        WHEN 1 THEN 'NUMERIC(' || F.RDB$FIELD_PRECISION || ', ' || (-F.RDB$FIELD_SCALE) || ')' "
+ "        WHEN 2 THEN 'DECIMAL' "
+ "      END "
+ "    WHEN 8 THEN "
+ "      CASE F.RDB$FIELD_SUB_TYPE "
+ "        WHEN 0 THEN 'INTEGER' "
+ "        WHEN 1 THEN 'NUMERIC(' || F.RDB$FIELD_PRECISION || ', ' || (-F.RDB$FIELD_SCALE) || ')' "
+ "        WHEN 2 THEN 'DECIMAL' "
+ "      END "
+ "    WHEN 9 THEN 'QUAD' "
+ "    WHEN 10 THEN 'FLOAT' "
+ "    WHEN 12 THEN 'DATE' "
+ "    WHEN 13 THEN 'TIME' "
+ "    WHEN 14 THEN 'CHAR(' || (TRUNC(F.RDB$FIELD_LENGTH / CH.RDB$BYTES_PER_CHARACTER)) || ') ' "
+ "    WHEN 16 THEN "
+ "      CASE F.RDB$FIELD_SUB_TYPE "
+ "        WHEN 0 THEN 'BIGINT' "
+ "        WHEN 1 THEN 'NUMERIC(' || F.RDB$FIELD_PRECISION || ', ' || (-F.RDB$FIELD_SCALE) || ')' "
+ "        WHEN 2 THEN 'DECIMAL' "
+ "      END "
+ "    WHEN 27 THEN 'DOUBLE' "
+ "    WHEN 35 THEN 'DATETIME' "
+ "    WHEN 37 THEN 'VARCHAR(' || (TRUNC(F.RDB$FIELD_LENGTH / CH.RDB$BYTES_PER_CHARACTER)) || ')' "
+ "    WHEN 40 THEN 'CSTRING' || (TRUNC(F.RDB$FIELD_LENGTH / CH.RDB$BYTES_PER_CHARACTER)) || ')' "
+ "    WHEN 45 THEN 'BLOB_ID' "
+ "    WHEN 261 THEN 'LONGBLOB '  "
+ "    ELSE 'RDB$FIELD_TYPE: ' || F.RDB$FIELD_TYPE || '?' "
+ "  END FIELD_TYPE, "
+ "  IIF(COALESCE(RF.RDB$NULL_FLAG, 0) = 0, NULL, 'NOT NULL') FIELD_NULL, "
+ "  CH.RDB$CHARACTER_SET_NAME FIELD_CHARSET, "
+ "  DCO.RDB$COLLATION_NAME FIELD_COLLATION, "
+ "  COALESCE(RF.RDB$DEFAULT_SOURCE, F.RDB$DEFAULT_SOURCE) FIELD_DEFAULT, "
+ "  F.RDB$VALIDATION_SOURCE FIELD_CHECK, "
+ "  RF.RDB$DESCRIPTION FIELD_DESCRIPTION "
+ "FROM RDB$RELATION_FIELDS RF "
+ "JOIN RDB$FIELDS F ON(F.RDB$FIELD_NAME = RF.RDB$FIELD_SOURCE) "
+ "LEFT OUTER JOIN RDB$CHARACTER_SETS CH ON(CH.RDB$CHARACTER_SET_ID = F.RDB$CHARACTER_SET_ID) "
+ "LEFT OUTER JOIN RDB$COLLATIONS DCO ON((DCO.RDB$COLLATION_ID = F.RDB$COLLATION_ID) AND(DCO.RDB$CHARACTER_SET_ID = F.RDB$CHARACTER_SET_ID)) "
+ $"WHERE(RF.RDB$RELATION_NAME = '{tableName}') AND(COALESCE(RF.RDB$SYSTEM_FLAG, 0) = 0) "
+ "ORDER BY RF.RDB$FIELD_POSITION; "

            ;
            //"SELECT" +
            //"  TRIM(rf.rdb$field_name) || ' ' || " +
            //"  IIF(rdb$field_source LIKE 'RDB$%', " +
            //"  DECODE(f.rdb$field_type, " +
            //"    8, 'INTEGER', " +
            //"    12, 'DATE', " +
            //"    37, 'VARCHAR', " +
            //"    14, 'CHAR', " +
            //"    7, 'SMALLINT'), " +
            //"  TRIM(rdb$field_source)) || " +
            //"  IIF((rdb$field_source LIKE 'RDB$%') AND(f.rdb$field_type IN(37, 14)), " +
            //"    '(' || f.rdb$field_length || ')', " +
            //"    '') || " +
            //"  IIF((f.rdb$null_flag = 1) OR(rf.rdb$null_flag = 1),  " +
            //"    ' NOT NULL', '') " +
            //"FROM " +
            //"  rdb$relation_fields rf JOIN rdb$fields f " +
            //"    ON f.rdb$field_name = rf.rdb$field_source " +
            //"WHERE " +
            //$"  rf.rdb$relation_name = '{tableName}'"
            //;
            FbCommand query = new FbCommand(sql, connFB);
            FbDataReader reader = query.ExecuteReader();
            List<string> columns = new List<string>();
            while (reader.Read())
            {
                changeLog($"get column's {tableName} types {reader.GetString(0).Replace(" ", "")} {reader.GetString(1).Replace(" ", "")} ");

                var null_status = reader.GetString(2) != null ? reader.GetString(2) : string.Empty;
                columns.Add($"{reader.GetString(0).Replace(" ", "")} {reader.GetString(1).Replace(" ", "")} {null_status}");

                //columns.Append(reader.GetString(1));

                //result.Add(tableName, reader.GetString(0));
            }


            return columns;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            changeLog("Opening Connection");
            openConnection();

            changeLog("getting Tabel List");

            List<string> views = getAllViews();
            List<string> tableList = getAllTableList();
            ListTablesViews listTablesViewsForm = new ListTablesViews(tableList, views);
            listTablesViewsForm.ShowDialog();
            if (listTablesViewsForm.isOK)
            {
                tableList = listTablesViewsForm.tables;
                views = listTablesViewsForm.views;
                backgroundWorker1.ReportProgress((int)(progress += 3f));
                Dictionary<string, List<string>> TableDesc = new Dictionary<string, List<string>>();

                //total harus 10%
                int columnCount = tableList.Count;
                float totalProgress = 10f;
                float progressDelta = (totalProgress * 1f / columnCount) - (totalProgress * 0f / columnCount);
                tableList.ForEach((tableName) =>
                {
                    changeLog($"get column and data type of table {tableName}");

                    TableDesc.Add(tableName, columnDesc(tableName));
                    backgroundWorker1.ReportProgress((int)(progress += progressDelta));

                });

                //MEMBUAT SQL CREATE TABLE COMMAND SYNTAX
                columnCount = tableList.Count;
                totalProgress = 10f;
                progressDelta = (totalProgress * 1f / columnCount) - (totalProgress * 0f / columnCount);
                int lastIndexColumn;

                tableList.ForEach((tableName) =>
                {
                    List<string> columnsThatHasPrimaryKey = getPrimaryKeys(tableName);

                    //iterasi tiap kolom pada tabelName
                    int primaryKeyCounter = 0;
                    for (int i = 0; i < TableDesc[tableName].Count; i++)
                    {
                        List<string> primaryKeys = new List<string>();
                        for (int j = 0; j < columnsThatHasPrimaryKey.Count; j++)
                        {
                            var columnPrimaryKey = columnsThatHasPrimaryKey[j];
                            var currentColumnName = TableDesc[tableName][i].Split(' ')[0];
                            if (TableDesc[tableName][i].Split(' ')[0] == columnPrimaryKey)
                            {
                                primaryKeyCounter++;
                                lastIndexColumn = TableDesc[tableName].Count - 1;
                                if (TableDesc[tableName][lastIndexColumn].IndexOf("primary key") == -1)
                                {
                                    TableDesc[tableName].Add(" primary key (");

                                }
                                lastIndexColumn = TableDesc[tableName].Count - 1;

                                TableDesc[tableName][lastIndexColumn] += currentColumnName;
                                if (columnsThatHasPrimaryKey.Count == 1)
                                {
                                    TableDesc[tableName][lastIndexColumn] += ")";
                                    continue;
                                }
                                if (primaryKeyCounter == columnsThatHasPrimaryKey.Count)
                                {
                                    TableDesc[tableName][lastIndexColumn] += ")";
                                }
                                else
                                {

                                    TableDesc[tableName][lastIndexColumn] += ", ";
                                }
                            }
                        }

                    }
                    lastIndexColumn = TableDesc[tableName].Count - 1;

                    //if(TableDesc[tableName][lastIndexColumn].IndexOf("primary key") > -1){
                    //    TableDesc[tableName][lastIndexColumn] += ")";
                    //}
                    backgroundWorker1.ReportProgress((int)(progress += progressDelta));


                });

                //CREATE TABLE
                totalProgress = 20f;
                int tableCount = TableDesc.Count;
                progressDelta = (totalProgress * 1f / tableCount) - (totalProgress * 0f / tableCount);
                createTables(TableDesc, progressDelta);

                //INSERT TABLE
                totalProgress = 40f;
                progressDelta = (totalProgress * 1f / tableCount) - (totalProgress * 0f / tableCount);
                insertTables(TableDesc, progressDelta);






                changeLog("Get Views Name");
                //VIEWS
                Dictionary<string, string> viewAllias = new Dictionary<string, string>();
                views.ForEach(view =>
                {
                    viewAllias.Add(view, getAlliasView(view));


                });
                totalProgress = 20f;
                string sql;
                foreach (var view in viewAllias)
                {
                    //Console.WriteLine("=================================");
                    sql = $"CREATE VIEW {view.Key.Replace(" ", "")} AS {view.Value}";
                    while (true)
                    {

                        //fixing the sql command. In mysql, cast () cannot have blank space after it

                        var words = sql.Split(' ');

                        sql = "";
                        for (int i = 0; i < words.Length; i++)
                        {
                            if (words[i] == "cast")
                            {
                                //ignore the blank space
                                sql += words[i];
                                sql += words[i + 1];
                                i++;
                            }
                            else
                            {
                                sql += " " + words[i];
                            }
                        }
                        //replacing numeric to decimal
                        sql = sql.Replace("numeric", "decimal");

                        Console.WriteLine(sql);

                        MySqlCommand query = new MySqlCommand(sql, connMysql);
                        try
                        {
                            query.ExecuteNonQuery();
                            break;
                        }
                        catch (Exception ex)
                        {

                            if (ex.Message.ToLower().IndexOf("duplicate") > -1)
                            {
                                changeLog("view contain duplicate column name, generating new column name...");
                                var columnName = ex.Message.Split(' ')[ex.Message.Split(' ').Length - 1].Replace("'", "");
                                //words = sql.Split(' ');
                                //int indexWord = sql.IndexOf(columnName);
                                int i = 0;
                                //ini loop terus sampai replacenya sudah sama dengan sql (maka artinyatidak ada lagi yang perlu direplace)
                                //problem: ini selalu ngereplace nama column pertama, tidak pernah nyentuh kolom berikutnya
                                while (sql != ReplaceFirst(sql,columnName,$"{columnName}{i}"))
                                {
                                    sql = ReplaceFirst(sql, columnName,$"{columnName}{i}");
                                    i++;

                                }

                                
                            }
                            else
                            {
                                Console.WriteLine(ex.Message);
                                break;
                            }

                        }
                    }
                    progressDelta = (totalProgress * 1f / viewAllias.Count) - (totalProgress * 0f / viewAllias.Count);
                    progress += progressDelta;
                    changeLog($"VIEW {view.Key} INSERTED");
                }
                progress = 100;
                changeLog("Done");
            }

            //Console.WriteLine(TableDesc);
        }
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
        private string getAlliasView(string viewName)
        {
            FbCommand query = new FbCommand($"select rdb$view_source from rdb$relations where rdb$relation_name = '{viewName.ToUpper()}' ", connFB);
            Console.WriteLine($"select rdb$view_source from rdb$relations where rdb$relation_name = '{viewName.ToUpper().Replace(" ", "")}'");
            readerFB = query.ExecuteReader();
            readerFB.Read();
            return readerFB.GetString(0);

        }
        private List<string> getAllViews()
        {
            List<string> result = new List<string>();

            FbCommand queryFB = new FbCommand("select rdb$relation_name from rdb$relations where rdb$view_blr is not null and (rdb$system_flag is null or rdb$system_flag = 0); ", connFB);
            readerFB = queryFB.ExecuteReader();
            while (readerFB.Read())
            {
                result.Add(readerFB.GetString(0));
            }
            readerFB.Close();
            return result;
        }
        private void InsertTableChildThread(string namaTabel)
        {

        }
        FbDataReader sqlReader;
        private void insertTables(Dictionary<string, List<string>> tableDesc, float progressDelta)
        {
            int index = 0;
            MySqlCommand queryMySql = new MySqlCommand("", connMysql);
            bool startNow = false;
            foreach (var table in tableDesc)
            {
                //if(config.source.start !=null)
                //    if(table.Key.ToUpper() == start)
                //    {
                //        startNow = true;
                //    }
                //if (!startNow)
                //{
                //    changeLog($"Skipping {table.Key}");
                //    continue;
                //}
                Console.WriteLine(table.Key);
                changeLog($"Inserting Table {table.Key}");
                FbCommand queryFB = new FbCommand($"SELECT * FROM {table.Key}", connFB);
                //queryFB.Parameters.Add("@test", "");
                var reader = queryFB.ExecuteReader();
                var result = "";
                int counterRow = 0;
                while (reader.Read())
                {
                    string sqlMysql = $"INSERT INTO {table.Key} VALUES(";
                    List<object> fieldValue = new List<object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {

                        if (reader.GetDataTypeName(i).ToUpper() == "VARCHAR" || reader.GetDataTypeName(i).ToUpper() == "CHAR")
                        {
                            //result = reader.GetString(i) != "" && reader.GetString(i) != null ? $"'{reader.GetString(i).Replace("'","''")}'" : "null";
                            result = reader.GetString(i) != "" && reader.GetString(i) != null ? $"{reader.GetString(i)}" : null;
                            //sqlMysql += $"{result},";
                            sqlMysql += $"@param{i},";
                            fieldValue.Add(result);
                            continue;
                        }
                        if (reader.GetDataTypeName(i).ToUpper() == "DATETIME" || reader.GetDataTypeName(i).ToUpper() == "DATE" || reader.GetDataTypeName(i).ToUpper() == "TIMESTAMP")
                        {
                            //result = reader.GetString(i) != "" && reader.GetString(i) != null ? $"\"{reader.GetString(i)}\"" : "null";
                            result = reader.GetString(i) != "" && reader.GetString(i) != null ? $"{reader.GetString(i)}" : null;

                            //if (result == "null")
                            if (result == null)
                            {
                                sqlMysql += $"@param{i},";
                                fieldValue.Add(null);

                                //sqlMysql += $"{result},"; //\"20/02/2010 00:00:00\"
                            }
                            else
                            {
                                try
                                {
                                    DateTime dateTime = DateTime.ParseExact(result.Replace("\"", ""), "dd/MM/yyyy HH:mm:ss", null);
                                    if (dateTime.Year >= 2999 || dateTime.Year <= 1000)
                                    {
                                        if (forceModeCheckBox.Checked)
                                        {
                                            result = null;
                                        }
                                        else
                                        {
                                            throw new Exception("incorrect Year");

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"{ex.Message}\n\n{result.Replace("'", "")}\nIn Table: {table.Key} \nDate: {result}\nRaw SQL: \n{sqlMysql}");
                                    MessageBox.Show($"{ex.Message}\n\n{result.Replace("'", "")}\nIn Table: {table.Key} \nDate: {result}\nRaw SQL: \n{sqlMysql}", "Error Parse date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                if (result != null)
                                {

                                    fieldValue.Add(result);
                                    if (result.IndexOf("-") != -1)
                                    {
                                        sqlMysql += $"STR_TO_DATE(@param{i},'%d-%m-%Y %H:%i:%S' ),";
                                        Console.WriteLine("contain -");

                                    }
                                    else
                                    {
                                        sqlMysql += $"STR_TO_DATE(@param{i},'%d/%m/%Y %H:%i:%S' ),";

                                    }
                                    //sqlMysql += $"STR_TO_DATE({result},'%d/%m/%Y %H:%i:%S' ),"; //'20/02/2010 00:00:00'
                                }
                                else
                                {
                                    fieldValue.Add(null);
                                    sqlMysql += $"@param{i},";

                                    //sqlMysql += $"{result},"; //'20/02/2010 00:00:00'

                                }
                            }


                            continue;
                        }
                        if (reader.GetDataTypeName(i).ToUpper() == "DECIMAL" || reader.GetDataTypeName(i).ToUpper() == "NUMERIC")
                        {
                            decimal resultDecimal;
                            try
                            {
                                if (reader.GetDecimal(i) != null)
                                {
                                    resultDecimal = reader.GetDecimal(i);
                                    //sqlMysql += $"{resultDecimal.ToString().Replace(',','.')},";
                                    fieldValue.Add(resultDecimal.ToString().Replace(',', '.'));
                                    sqlMysql += $"@param{i},";

                                }
                                else
                                {
                                    fieldValue.Add(null);
                                    sqlMysql += $"@param{i},";
                                }

                            }
                            catch (Exception ex)
                            {
                                if (!forceModeCheckBox.Checked)
                                    MessageBox.Show($"{ex.Message} \ndataString: {reader.GetString(i)}", "Error Parsing To Decimal", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                fieldValue.Add(null);
                                sqlMysql += $"@param{i},";
                                //Console.WriteLine($"{ex.Message} \ndataString: {reader.GetString(i)}");

                            }
                            continue;
                        }

                        result = reader.GetString(i) != null && reader.GetString(i) != string.Empty ? reader.GetString(i) : null;
                        fieldValue.Add(result);
                        sqlMysql += $"@param{i},";
                    }
                    StringBuilder sb = new StringBuilder(sqlMysql);
                    sb[sqlMysql.Length - 1] = ' ';
                    sqlMysql = sb.ToString();
                    sqlMysql += ")";
                    try
                    {
                        queryMySql.CommandText = sqlMysql;
                        counterRow++;
                        if (counterRow % 10 == 0)
                        {
                            changeLog($"inserted {counterRow} row(s) of {table.Key}");
                        }
                        queryMySql.Parameters.Clear();
                        for (int j = 0; j < fieldValue.Count; j++)
                        {
                            queryMySql.Parameters.AddWithValue($"@param{j}", fieldValue[j]);
                            //queryMySql.Parameters.AddWithValue("","");
                        }
                        var res = queryMySql.ExecuteNonQuery();

                    }
                    catch (Exception ex)
                    {
                        if (ignoreDuplicateKey.Checked)
                        {
                            if (ex.Message.ToLower().IndexOf("duplicate") != -1)
                            {
                                //changeLog($"Duplicate Entry, Skipping {sqlMysql}");
                                //Console.WriteLine($"Duplicate Entry, Skipping {sqlMysql}");
                            }
                            else
                            {
                                if (!forceModeCheckBox.Checked)
                                {
                                    MessageBox.Show($"{ex.Message} \n {sqlMysql}", "Data Mismatch!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Application.Exit();

                                }
                                Console.WriteLine($"{ex.Message} \n {sqlMysql}");
                                for (int j = 0; j < fieldValue.Count; j++)
                                {
                                    Console.WriteLine(fieldValue[j]);
                                }
                                //Console.WriteLine($"{ex.Message} \n {sqlMysql}");

                            }
                        }
                        else
                        {
                            MessageBox.Show($"{ex.Message} \n {sqlMysql}", "Data Mismatch!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        //Console.WriteLine($"{ex.Message} \n {sqlMysql}");
                    }
                }
                backgroundWorker1.ReportProgress((int)(progress += progressDelta));
                index++;
                //Console.WriteLine($"{index}/{tableDesc.Count}");
                changeLog($"{index}/{tableDesc.Count}");
            }
            backgroundWorker1.ReportProgress(100);
        }
        private void createTables(Dictionary<string, List<string>> tableDesc, float progressDelta)
        {
            int countTable = 0;
            foreach (var table in tableDesc)
            {
                string sql =
                    $"CREATE TABLE {table.Key} (";
                for (int i = 0; i < table.Value.Count; i++)
                {
                    sql += table.Value[i];
                    if (i + 1 != table.Value.Count)
                    {
                        sql += ",";
                    }
                }
                sql += ");";
                countTable++;
                try
                {
                    MySqlCommand query = new MySqlCommand(sql, connMysql);
                    var res = query.ExecuteNonQuery();

                }
                catch (Exception EX)
                {
                    if (!ignoreExistCheckBox.Checked)
                    {
                        MessageBox.Show((EX.Message), $"Error Create Table {table.Key}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    Console.WriteLine(EX.Message);
                }
                backgroundWorker1.ReportProgress((int)(progress += progressDelta));
            };
        }
        private void button1_Click(object sender, EventArgs e)
        {
            progress = 0f;
            backgroundWorker1.RunWorkerAsync();
        }
        string tempLog = "";
        private void changeLog(string log)
        {
            tempLog = log;
            backgroundWorker1.ReportProgress((int)progress);
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            LogForm.changePercentageProgress(e.ProgressPercentage);
            logLabel.Text = tempLog;
            LogForm.appendLog(tempLog);

        }
        Log LogForm = new Log();
        private void showLog_Click(object sender, EventArgs e)
        {
            LogForm.ShowDialog();

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (forceModeCheckBox.Checked)
            {
                ignoreDuplicateKey.Checked = true;
                ignoreExistCheckBox.Checked = true;
            }
            else
            {
                ignoreDuplicateKey.Checked = false;
                ignoreExistCheckBox.Checked = false;

            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
