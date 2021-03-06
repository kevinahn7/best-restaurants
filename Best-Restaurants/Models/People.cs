﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BestRestaurants.Models
{
    public class People
    {
        public int PeopleId { get; set; }
        public string PeopleName { get; set; }

        public People(string PeopleName, int Id = 0)
        {
            this.PeopleId = Id;
            this.PeopleName = PeopleName;
        }

        public override bool Equals(System.Object otherPeople)
        {
            if (!(otherPeople is People))
            {
                return false;
            }
            else
            {
                People newPeople = (People)otherPeople;
                bool peopleIdEquality = (this.PeopleId == newPeople.PeopleId);
                bool peopleNameEquality = (this.PeopleName == newPeople.PeopleName);
                return (peopleIdEquality && peopleNameEquality);
            }
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM people;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<People> GetAll()
        {
            List<People> allPeople = new List<People> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM people;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int peopleId = rdr.GetInt32(0);
                string peopleName = rdr.GetString(1);
                People newPeople = new People(peopleName, peopleId);
                allPeople.Add(newPeople);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allPeople;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO people (name) VALUES (@PeopleName);";

            cmd.Parameters.AddWithValue("@PeopleName", this.PeopleName);

            cmd.ExecuteNonQuery();
            this.PeopleId = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static People Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `people` WHERE id = @thisId;";

            cmd.Parameters.AddWithValue("@thisId", id);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int peopleId = 0;
            string peopleName = "";

            while (rdr.Read())
            {
                peopleId = rdr.GetInt32(0);
                peopleName = rdr.GetString(1);
            }

            People foundItem = new People(peopleName, peopleId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundItem;
        }
    }
}
