﻿using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using static $safeprojectname$.Log;

namespace $safeprojectname$
{
    public class Dapper
    {
        public class Account
        {
            public string username = "";
            public string password = "";

            // 特殊建構式有定義例如：public Account(string user, string pass)，一定要定義 default 建構式，這樣才能正常執行
            public Account()
            {
            }

            public Account(string user, string pass)
            {
                username = user;
                password = pass;
            }
        }

        public class Logger
        {
            public string createtime = "";
            public string log = "";

            // 特殊建構式有定義例如：public Logger(string create, string log)，一定要定義 default 建構式，這樣才能正常執行
            public Logger()
            {
            }

            public Logger(string create, string log)
            {
                createtime = create;
                this.log = log;
            }
        }

        public static string _dbPath = Directory.GetCurrentDirectory() + @"/database.sqlite";
        static string _cnStr = string.Format(@"data source={0}", _dbPath);

        static string _serverIP = "inspireq-digitalassets.cfr4ol3udric.us-west-2.rds.amazonaws.com";
        static string _username = "IQ_DA_DB_Admin";
        static string _password = "holoportation";
        static string _database = "so";
        public static string _cnMySqlStr = string.Format("server={0};port=3306;user id={1};password={2};database={3};charset=utf8;", _serverIP, _username, _password, _database);
        // 連接 MySQL 只要把 SQLiteConnection 改成 MySqlConnection 就可以了

        static bool isTest = false;

        // 測試 SQlite database 與 Dapper
        public static void DapperTest()
        {
            if (isTest == false)
            {
                return;// 要測試設定 isTest 為 true
            }

            // SQLite 初始化資料庫
            InitSQLite();

            Account accInsert_ = new Account(@"tempUser", @"tempPass");
            InsertAccount(accInsert_);
            //InsertSQLite(accInsert_);
            // Dapper 查詢
            SelectAccount();
            //SelectSQLite();

            Account accUpdate_ = new Account(@"tempUser", @"tempNopass");
            UpdateAccount(accUpdate_);
            // Dapper 查詢
            SelectAccount();
            //SelectSQLite();

            DeleteAccount(@"tempUser");
            //DeleteSQLite(@"tempUser");
            // Dapper 查詢
            SelectAccount();
            //SelectSQLite();
        }

        // SQLite 初始化資料庫
        public static int InitSQLite()
        {
            if (File.Exists(_dbPath))
            {
                Print("資料庫已存在：" + _dbPath);
                return -1;
            }

            using (var cn_ = new SQLiteConnection(_cnStr))
            {
                cn_.Open();
                string sql = @"
                CREATE TABLE 'account1' 
                (   
                    'username' TEXT, 
                    'password' TEXT 
                )";
                SQLiteCommand cmd = new SQLiteCommand(sql, cn_);
                return cmd.ExecuteNonQuery();
            }
        }

        // SQLite 初始化 My 資料庫
        public static int InitMySQLite()
        {
            if (File.Exists(_dbPath))
            {
                Print("資料庫已存在：" + _dbPath);
                return -1;
            }

            using (var cn_ = new SQLiteConnection(_cnStr))
            {
                cn_.Open();
                string sql = @"
                CREATE TABLE 'account' 
                (   
                    'username' TEXT, 
                    'password' TEXT 
                )";
                SQLiteCommand cmd = new SQLiteCommand(sql, cn_);
                cmd.ExecuteNonQuery();
            }

            using (var cn_ = new SQLiteConnection(_cnStr))
            {
                cn_.Open();
                string sql = @"
                CREATE TABLE 'logger' 
                (   
                    'createtime' TEXT, 
                    'log' TEXT 
                )";
                SQLiteCommand cmd = new SQLiteCommand(sql, cn_);
                cmd.ExecuteNonQuery();
            }

            return 0;
        }

        // SQLite 新增
        public static int InsertSQLite(Account acc)
        {
            using (var cn_ = new SQLiteConnection(_cnStr))
            {
                cn_.Open();
                string sql_ = string.Format(@"insert into account (username,password) values('{0}','{1}')", acc.username, acc.password);
                SQLiteCommand cmd_ = new SQLiteCommand(sql_, cn_);
                return cmd_.ExecuteNonQuery();
            }
        }

        // SQLite 初始化資料庫
        public static int DeleteSQLite(string user)
        {
            //Print(_cnStr);
            using (var cn_ = new SQLiteConnection(_cnStr))
            {
                cn_.Open();
                string sql_ = string.Format(@"delete from account where username='{0}'", user);
                SQLiteCommand cmd_ = new SQLiteCommand(sql_, cn_);
                return cmd_.ExecuteNonQuery();
            }
        }

        // SQLite 查詢
        static void SelectSQLite()
        {
            using (var cn_ = new SQLiteConnection(_cnStr))
            {
                cn_.Open();
                SQLiteCommand cmd_ = new SQLiteCommand(@"select * from account", cn_);
                using (SQLiteDataReader sreader = cmd_.ExecuteReader())
                {
                    while (sreader.Read())
                    {
                        for (int i = 0; i < sreader.FieldCount; i++)
                        {
                            Print(string.Format("{0} : {1}", sreader.GetName(i), sreader.GetValue(i)));
                        }
                        Print("\n");
                    }
                }
            }
        }

        // Dapper 查詢
        public static List<Account> SelectAccount()
        {
            //Print(_cnStr);
            using (IDbConnection cnn_ = new SQLiteConnection(_cnStr))
            {
                cnn_.Open();
                var output_ = cnn_.Query<Account>(@"select * from account", new DynamicParameters());

                List<Account> acc_ = output_.ToList();
                acc_.ForEach(x => Print(string.Format(@"username={0} password={1}", x.username, x.password)));
                return acc_;
            }
        }

        // Dapper 查詢
        public static List<Account> SelectAccount(string sql)
        {
            //Print(_cnStr);
            using (IDbConnection cnn_ = new SQLiteConnection(_cnStr))
            {
                cnn_.Open();
                var output_ = cnn_.Query<Account>(sql, new DynamicParameters());

                List<Account> acc_ = output_.ToList();
                acc_.ForEach(x => Print(string.Format(@"username={0} password={1}", x.username, x.password)));
                return acc_;
            }
        }

        // Dapper 新增
        public static void InsertAccount(Account acc)
        {
            //Print(_cnStr);
            using (IDbConnection cnn_ = new SQLiteConnection(_cnStr))
            {
                cnn_.Open();
                string sql_ = string.Format(@"insert into account (username,password) values('{0}','{1}')", acc.username, acc.password);
                cnn_.Execute(sql_);
            }
        }

        // Dapper 新增
        public static void InsertLogger(Logger log)
        {
            //Print(_cnStr);
            using (IDbConnection cnn_ = new SQLiteConnection(_cnStr))
            {
                cnn_.Open();
                string sql_ = string.Format(@"insert into logger (createtime,log) values('{0}','{1}')", log.createtime, log.log);
                cnn_.Execute(sql_);
            }
        }

        // Dapper 刪除
        public static int DeleteAccount(string user)
        {
            //Print(_cnStr);
            using (IDbConnection cnn_ = new SQLiteConnection(_cnStr))
            {
                cnn_.Open();
                string sql_ = string.Format(@"delete from account where username='{0}'", user);
                return cnn_.Execute(sql_);
            }
        }

        public static void UpdateAccount(Account acc)
        {
            //Print(_cnStr);
            using (IDbConnection cnn_ = new SQLiteConnection(_cnStr))
            {
                cnn_.Open();
                string sql_ = string.Format(@"update account set password = '{0}' where username='{1}'", acc.password, acc.username);
                cnn_.Execute(sql_, acc);
            }
        }
    }
}
