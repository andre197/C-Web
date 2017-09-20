namespace School_Competition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        public static void Main(string[] args)
        {
            Dictionary<string, int> studentScore = new Dictionary<string, int>();
            Dictionary<string, SortedSet<string>> studentCategory = new Dictionary<string, SortedSet<string>>();

            while (true)
            {
                var input = Console.ReadLine().Split();

                if (input[0] == "END")
                {
                    break;
                }

                string name = input[0];
                string category = input[1];
                int score = int.Parse(input[2]);

                if (!studentScore.ContainsKey(name))
                {
                    studentScore[name] = 0;
                }

                if (!studentCategory.ContainsKey(name))
                {
                    studentCategory[name] = new SortedSet<string>();
                }

                studentScore[name] += score;
                studentCategory[name].Add(category);
            }

            Dictionary<string,int> orderedStudentScore = studentScore
                                            .OrderByDescending(kvp => kvp.Value)
                                            .ThenBy(kvp => kvp.Key)
                                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (KeyValuePair<string,int> kvp in orderedStudentScore)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value} [{string.Join(", ", studentCategory[kvp.Key])}]");
            }
        }
    }
}