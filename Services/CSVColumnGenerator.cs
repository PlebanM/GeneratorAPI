﻿using com.sun.net.httpserver;
using DataGenerator.Models;
using DataGenerator.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Services
{
    public class CSVColumnGenerator
    {
        private DataContext dataContext;
        private Dictionary<string, Func<Dictionary<string, int>, long, List<string>>> generatorFunctions;

        public CSVColumnGenerator(DataContext dataContext)
        {
            this.dataContext = dataContext;
            this.generatorFunctions = new Dictionary<string, Func<Dictionary<string, int>, long, List<string>>>();
            RegisterFunctions();
            
        }

        internal List<string> GenerateColumn(ColumnStructure columnStructure, long length)
        {
            if (generatorFunctions.ContainsKey(columnStructure.Type))
            {
                return generatorFunctions[columnStructure.Type](columnStructure.Options, length);
            }

            throw new ArgumentException();
            
        }

        private void RegisterFunctions()
        {
            generatorFunctions["lastName"] = GenerateLastNames;
            generatorFunctions["firstName"] = GenerateFirstNames;
            generatorFunctions["integer"] = GenerateIntegers;
            generatorFunctions["email"] = GenerateEmails;
            generatorFunctions["randomWord"] = GenerateRandomString;

        }

        private List<string> GenerateEmails(Dictionary<string, int> options, long length)
        {

            HashSet<String> result = new HashSet<string>();

            StringBuilder sb = new StringBuilder();
            Random rand = new Random();

            int firstNameDBSize = dataContext.FirstNames.Count();
            int lastNameDBSize = dataContext.LastNames.Count();
            int domainsDBSize = dataContext.Domains.Count();
            var firstnameDB = GenerateFirstNames(options, firstNameDBSize);
            var lastNameDB = GenerateLastNames(options, lastNameDBSize);
            var domainsDB = GenerateDomains(options, domainsDBSize);

            string sbToString;

            int toSkipFirstName, toSkipLastName, toSkipDomain = 0;
            int numberToUniqueChangeEmail = 0;
            while (result.Count < length)
            {
                toSkipFirstName = rand.Next(0, firstNameDBSize);
                toSkipLastName = rand.Next(0, lastNameDBSize);
                toSkipDomain = rand.Next(0, domainsDBSize);

                sb.Append(firstnameDB.ElementAt(toSkipFirstName));
                sb.Append(".");
                sb.Append(lastNameDB.ElementAt(toSkipLastName));
                sb.Append("@");
                sb.Append(domainsDB.ElementAt(toSkipDomain));

                sbToString = sb.ToString();

                if (result.Contains(sbToString))
                {
                    int atIndex = sbToString.IndexOf("@");
                    sb.Insert(atIndex - 1, numberToUniqueChangeEmail++);
                }

                result.Add(sb.ToString());
                sb.Clear();
            }

            return result.ToList();
        }

        public List<string> GenerateDomains(Dictionary<string, int> options, long length)
        {
            var result = new List<string>();
            while (result.Count < length)
            {
                result.AddRange(from domains in dataContext.Domains
                                select domains.Name);
            }
            return result;
        }

        public List<string> GenerateLastNames(Dictionary<string, int> options, long length)
        {
            var result = new List<string>();
            while (result.Count < length)
            {
                result.AddRange(from lastName in dataContext.LastNames
                                select lastName.Name);
            }
            return result;
        }

        public List<string> GenerateFirstNames(Dictionary<string, int> options, long length)
        {
            var result = new List<string>();

            
            while (result.Count < length)
            {
                result.AddRange(from firstName in dataContext.FirstNames
                                select firstName.Name);
            }
            return result;
        }

        public List<string> GenerateIntegers(Dictionary<string, int> options, long length)
        {
            int optionsFrom = options.GetValueOrDefault("from", 1);      
            var optionsGap = options.GetValueOrDefault("gap", 1);

            var result = new List<string>();

           
            //    throw new BaseCustomException("To big gap", "Gap bigger than ", 400);
            

            while (result.Count() < length)

            {
                result.Add(optionsFrom.ToString());
                optionsFrom += optionsGap;
            }
            
            return result;
        }


        public List<string> GenerateRandomString(Dictionary<string, int> options, long length)
        {
            return null;
        }

    }
}
