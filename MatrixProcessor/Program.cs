using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixProcessor
{
    class Program
    {
        private const char delimiter = ',';
        static void Main(string[] args)
        {
            FileInfo fi = new FileInfo(args[0]);
            //int[cause, effect]
            Dictionary<int[], int> matrix = new Dictionary<int[], int>();
            
            using (var streamReader = fi.OpenText())
            {
                string eventLine;
                while ((eventLine = streamReader.ReadLine()) != null)
                {
                    AddEventToMatrix(eventLine, matrix);
                }
            }

            PrintMatrix(matrix);
       
        }

        private static void PrintMatrix(Dictionary<int[], int> matrix)
        {
            Console.WriteLine("cause, effect, frequency");
            foreach (var keyPair in matrix.Keys)
            {
                Console.WriteLine(string.Format("{0}, {1}, {2}", keyPair[0], keyPair[1], matrix[keyPair])); 
            }
        }

        private static void AddEventToMatrix(string eventLine, Dictionary<int[], int> matrix)
        {
            string[] eventColumns = eventLine.Split(new char[] { delimiter });
            
            // at least 2 evetn IDs
            if (eventColumns.Length > 5)
            {
                int eventCause = Int32.Parse(eventColumns[4]);
                int eventEffect = Int32.Parse(eventColumns[eventColumns.Length - 1]);
                int[] eventPair = new int[] { eventCause, eventEffect };
                int[] eventKey;
                if ( (eventKey = GetArrayKeyPair(matrix, eventPair)) != null)
                {
                    matrix[eventKey] += 1;
                }
                else
                {
                    matrix.Add(eventPair, 1);
                }
            }
        }

        private static int[] GetArrayKeyPair(Dictionary<int[], int> matrix, int[] keyPair)
        {
            int[] keytoReturn = null;
            foreach (var key in matrix.Keys)
            {
                if (key.Length == keyPair.Length)
                {
                    bool allEqual = true;
                    for (int i=0; i < key.Length; i++)
                    {
                        if (key[i] != keyPair[i])
                        {
                            allEqual = false;
                        }
                    }

                    if (allEqual)
                    {
                        keytoReturn = key;
                    }
                }
            }

            return keytoReturn;
        }
    }
}
