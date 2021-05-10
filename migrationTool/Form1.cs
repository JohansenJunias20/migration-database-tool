﻿using System;
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
        public Form1()
        {
            InitializeComponent();
            toolTip2.SetToolTip(ignoreExistCheckBox, "ignore warning table already exist. If your mysql database target already has tables you can check this checkbox");
            toolTipDuplicateKey.SetToolTip(ignoreDuplicateKey, "ignore warning data/row already exist. If your mysql database target already has several same data with firebird source database you can check this checkbox");
            toolTipDuplicateKey.SetToolTip(forceModeCheckBox, "ignore all exception");
            var config = System.IO.File.ReadAllText("./config.json");
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(config);
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Open Connection MYSQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine(ex.Message);
            }
            changeLog("sucess Open MYSQL Connection");

            backgroundWorker1.ReportProgress((int)(progress+=3));
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
            catch(Exception ex)
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
                changeLog($"read Tabel List Firebird: {readerFB.GetString(0).Replace(" ","")}");

                tableList.Add(readerFB.GetString(0).Replace(" ",""));
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
                result.Add(reader.GetString(0).Replace(" ",""));
            }
            return result;
        }
        List<string> columnDesc(string tableName)
        {

            string sql =
                "SELECT "
+"  RF.RDB$FIELD_NAME FIELD_NAME, "
+"  CASE F.RDB$FIELD_TYPE "
+"    WHEN 7 THEN "
+"      CASE F.RDB$FIELD_SUB_TYPE "
+"        WHEN 0 THEN 'SMALLINT' "
+"        WHEN 1 THEN 'NUMERIC(' || F.RDB$FIELD_PRECISION || ', ' || (-F.RDB$FIELD_SCALE) || ')' "
+"        WHEN 2 THEN 'DECIMAL' "
+"      END "
+"    WHEN 8 THEN "
+"      CASE F.RDB$FIELD_SUB_TYPE "
+"        WHEN 0 THEN 'INTEGER' "
+"        WHEN 1 THEN 'NUMERIC(' || F.RDB$FIELD_PRECISION || ', ' || (-F.RDB$FIELD_SCALE) || ')' "
+"        WHEN 2 THEN 'DECIMAL' "
+"      END "
+"    WHEN 9 THEN 'QUAD' "
+"    WHEN 10 THEN 'FLOAT' "
+"    WHEN 12 THEN 'DATE' "
+"    WHEN 13 THEN 'TIME' "
+"    WHEN 14 THEN 'CHAR(' || (TRUNC(F.RDB$FIELD_LENGTH / CH.RDB$BYTES_PER_CHARACTER)) || ') ' "
+"    WHEN 16 THEN "
+"      CASE F.RDB$FIELD_SUB_TYPE "
+"        WHEN 0 THEN 'BIGINT' "
+"        WHEN 1 THEN 'NUMERIC(' || F.RDB$FIELD_PRECISION || ', ' || (-F.RDB$FIELD_SCALE) || ')' "
+"        WHEN 2 THEN 'DECIMAL' "
+"      END "
+"    WHEN 27 THEN 'DOUBLE' "
+"    WHEN 35 THEN 'TIMESTAMP' "
+"    WHEN 37 THEN 'VARCHAR(' || (TRUNC(F.RDB$FIELD_LENGTH / CH.RDB$BYTES_PER_CHARACTER)) || ')' "
+"    WHEN 40 THEN 'CSTRING' || (TRUNC(F.RDB$FIELD_LENGTH / CH.RDB$BYTES_PER_CHARACTER)) || ')' "
+"    WHEN 45 THEN 'BLOB_ID' "
+ "    WHEN 261 THEN 'LONGBLOB '  "
+ "    ELSE 'RDB$FIELD_TYPE: ' || F.RDB$FIELD_TYPE || '?' "
+"  END FIELD_TYPE, "
+"  IIF(COALESCE(RF.RDB$NULL_FLAG, 0) = 0, NULL, 'NOT NULL') FIELD_NULL, "
+"  CH.RDB$CHARACTER_SET_NAME FIELD_CHARSET, "
+"  DCO.RDB$COLLATION_NAME FIELD_COLLATION, "
+"  COALESCE(RF.RDB$DEFAULT_SOURCE, F.RDB$DEFAULT_SOURCE) FIELD_DEFAULT, "
+"  F.RDB$VALIDATION_SOURCE FIELD_CHECK, "
+"  RF.RDB$DESCRIPTION FIELD_DESCRIPTION "
+"FROM RDB$RELATION_FIELDS RF "
+"JOIN RDB$FIELDS F ON(F.RDB$FIELD_NAME = RF.RDB$FIELD_SOURCE) "
+"LEFT OUTER JOIN RDB$CHARACTER_SETS CH ON(CH.RDB$CHARACTER_SET_ID = F.RDB$CHARACTER_SET_ID) "
+"LEFT OUTER JOIN RDB$COLLATIONS DCO ON((DCO.RDB$COLLATION_ID = F.RDB$COLLATION_ID) AND(DCO.RDB$CHARACTER_SET_ID = F.RDB$CHARACTER_SET_ID)) "
+$"WHERE(RF.RDB$RELATION_NAME = '{tableName}') AND(COALESCE(RF.RDB$SYSTEM_FLAG, 0) = 0) "
+"ORDER BY RF.RDB$FIELD_POSITION; "

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
            List<string> columns =new  List<string>();
            while (reader.Read())
            {
                changeLog($"get column's {tableName} types {reader.GetString(0).Replace(" ", "")} {reader.GetString(1).Replace(" ", "")} ");

                var null_status = reader.GetString(2) != null ? reader.GetString(2) : string.Empty;
                    columns.Add($"{reader.GetString(0).Replace(" ","")} {reader.GetString(1).Replace(" ","")} {null_status}");
                  
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
            List<string> tableList = getAllTableList();
            backgroundWorker1.ReportProgress((int)( progress += 3f));
            Dictionary<string, List<string>> TableDesc = new Dictionary<string, List<string>>();

            //total harus 5%
            int columnCount = tableList.Count;
            float totalProgress = 10f;
            float progressDelta = (totalProgress * 1f/columnCount )-  (totalProgress * 0f / columnCount);
            tableList.ForEach((tableName) =>
            {
                changeLog($"get column and data type of table {tableName}");

                TableDesc.Add(tableName,columnDesc(tableName));
                backgroundWorker1.ReportProgress((int) (progress += progressDelta));
                
            });

            //total harus 10%
            columnCount = tableList.Count;
            totalProgress = 15f;
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
                            if(columnsThatHasPrimaryKey.Count == 1)
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
                backgroundWorker1.ReportProgress((int) (progress += progressDelta));


            });
            totalProgress = 20f;
            int tableCount = TableDesc.Count;
            progressDelta = (totalProgress * 1f / tableCount) - (totalProgress * 0f / tableCount);
            createTables(TableDesc,progressDelta);


            totalProgress = 40f;
            progressDelta = (totalProgress * 1f / tableCount) - (totalProgress * 0f / tableCount);
            insertTables(TableDesc, progressDelta);
            //Console.WriteLine(TableDesc);
        }
        private void InsertTableChildThread(string namaTabel)
        {

        }
        private void insertTables(Dictionary<string,List<string>> tableDesc, float progressDelta)
        {
            int index = 0;
            foreach(var table in tableDesc)
            {


                Console.WriteLine(table.Key);
                FbCommand queryFB = new FbCommand($"SELECT * FROM {table.Key}", connFB);
                var reader = queryFB.ExecuteReader();
                var result = "";
                Thread thr = new Thread(()=>InsertTableChildThread());
                thr.Start();
                while (reader.Read())
                {
                    string sqlMysql = $"INSERT INTO {table.Key} VALUES(";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if(reader.GetDataTypeName(i).ToUpper() == "VARCHAR"|| reader.GetDataTypeName(i).ToUpper() == "CHAR")
                        {
                            result = reader.GetString(i) != "" && reader.GetString(i) != null ? $"`{reader.GetString(i)}`" : "null";
                            sqlMysql += $"{result},";
                            continue;
                        }
                        if(reader.GetDataTypeName(i).ToUpper() == "DATETIME" || reader.GetDataTypeName(i).ToUpper() == "DATE" || reader.GetDataTypeName(i).ToUpper() == "TIMESTAMP")
                        {
                            result = reader.GetString(i) != "" && reader.GetString(i) != null ? $"`{reader.GetString(i)}`" : "null";
                            
                           
                                if (result == "null")
                                {
                                    sqlMysql += $"{result},"; //`20/02/2010 00:00:00`
                                }
                                else
                                {
                                    try
                                    {
                                        DateTime dateTime = DateTime.ParseExact(result.Replace("'",""), "dd/MM/yyyy HH:mm:ss", null);
                                        if (dateTime.Year >= 2999||dateTime.Year <= 1000)
                                        {
                                            if (forceModeCheckBox.Checked)
                                            {
                                                result = "null";
                                            }
                                            else
                                            {
                                                throw new Exception("incorrect Year");

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"{ex.Message}\n\n{result}\nIn Table: {table.Key} \nDate: {result}\nRaw SQL: \n{sqlMysql}", "Error Parse date", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        Console.WriteLine($"{ex.Message}\n\n{result}\nIn Table: {table.Key} \nDate: {result}\nRaw SQL: \n{sqlMysql}");
                                    }

                                    if (result != "null")
                                    {
                                        sqlMysql += $"STR_TO_DATE({result},'%d/%m/%Y %H:%i:%S' ),"; //'20/02/2010 00:00:00'
                                    }
                                    else
                                    {
                                        sqlMysql += $"{result},"; //'20/02/2010 00:00:00'

                                    }
                                }

                         
                            continue;
                        }
                        if (reader.GetDataTypeName(i).ToUpper() == "DECIMAL" || reader.GetDataTypeName(i).ToUpper() == "NUMERIC" )
                        {
                            decimal resultDecimal;
                            try
                            {
                                if(reader.GetDecimal(i) != null)
                                {
                                    resultDecimal = reader.GetDecimal(i);
                                    sqlMysql += $"{resultDecimal.ToString().Replace(',','.')},";
                                }
                                else
                                {
                                    sqlMysql += $"null,";
                                }

                            }
                            catch(Exception ex)
                            {
                                if(!forceModeCheckBox.Checked)
                                    MessageBox.Show($"{ex.Message} \ndataString: {reader.GetString(i)}","Error Parsing To Decimal", MessageBoxButtons.OK,MessageBoxIcon.Error);
                                sqlMysql += $"null,";
                                Console.WriteLine($"{ex.Message} \ndataString: {reader.GetString(i)}");

                            }
                            continue;
                        }
                        result = reader.GetString(i) != null && reader.GetString(i) !=string.Empty ? reader.GetString(i) : "null";
                        sqlMysql += $"{result},";
                    }
                    StringBuilder sb = new StringBuilder(sqlMysql);
                    sb[sqlMysql.Length - 1] = ' ';
                    sqlMysql = sb.ToString();
                    sqlMysql += ")";
                    try
                    {
                    MySqlCommand queryMySql = new MySqlCommand(sqlMysql, connMysql);
                    var res = queryMySql.ExecuteNonQuery();

                    }
                    catch(Exception ex)
                    {
                        if (ignoreDuplicateKey.Checked)
                        {
                            if (ex.Message.ToLower().IndexOf("duplicate entry") > -1)
                            {
                                changeLog($"Duplicate Entry, Skipping {sqlMysql}");
                            }
                            else
                            {
                                if (!forceModeCheckBox.Checked)
                                {
                                    MessageBox.Show($"{ex.Message} \n {sqlMysql}","Data Mismatch!",MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Application.Exit();

                                }
                                Console.WriteLine($"{ex.Message} \n {sqlMysql}");

                            }
                        }
                        else
                        {
                            MessageBox.Show($"{ex.Message} \n {sqlMysql}", "Data Mismatch!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }
                        Console.WriteLine($"{ex.Message} \n {sqlMysql}");
                    }
                }
                backgroundWorker1.ReportProgress((int)(progress+=progressDelta));
                index++;
                Console.WriteLine($"{index}/{tableDesc.Count}");
            }
        }
        private void createTables(Dictionary<string,List<string>> tableDesc, float progressDelta)
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
                catch(Exception EX)
                {
                    if (!ignoreExistCheckBox.Checked)
                    {
                        MessageBox.Show((EX.Message),$"Error Create Table {table.Key}",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    Console.WriteLine(EX.Message);
                }
                backgroundWorker1.ReportProgress((int)(progress+=progressDelta));
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
    }
}
