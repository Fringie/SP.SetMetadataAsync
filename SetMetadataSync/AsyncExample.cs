using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetMetadataSync
{
    class AsyncExample
    {
        public void Main()
        {
            T(); // wil run instantly. synchronous
            var t = MyMethodAsync(); // will run task, synchronous
            L(); // wil run instantly, synchronous
            t.Wait(); // wil wait
            Console.WriteLine(t.Result);
        }

        void T()
        {
            Console.WriteLine("First");
        }

        void L()
        {
            Console.WriteLine("Last");
        }

        async Task<int> MyMethodAsync()
        {
            Task<int> longRunningTask = LongRunningOperationAsync();
            // independent work which doesn't need the result of LongRunningOperationAsync can be done here

            //and now we call await on the task 
            int result = await longRunningTask;
            //use the result 
            Console.WriteLine(result);
            return 55;
        }

        private async Task<int> LongRunningOperationAsync() // assume we return an int from this long running operation 
        {
            await Task.Delay(1000); // 1 second delay
            return 1;
        }
    }
}
