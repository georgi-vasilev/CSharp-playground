namespace ExpressionTrees
{
    using System;
    using System.Linq.Expressions;

    public class Program
    {
        class SomeHiddenClass
        {
            public int id = 42;
        }

        public static void Main(string[] args)
        {
            #region notes
            // Difference between Expression and Func
            // Func can be invoked, expressions cannot be invoked

            //Func<int, int, bool> func = (x, y) => x < y;
            //Expression<Func<int,int,bool>> expr = (x, y) => func(x, y);

            // Expression trees are DS representing code and we can analize it
            // and extract information from it

            // When we are using data storeges like SQL server, data is not in-memory
            // EF core tries to extract information from the expression tree and tries to
            // translate it to SQL

            // The only slow thing with Expression we can do is compiling them.
            // Writing variable that equals to 42 and passing directly 42 is not the same thing!
            // EF core has a tricky way to extract the value from variables.
            // Best practice is to compile as less as possible and if we need to compile it
            // it is better to cache it in a private field/dictionary or whatever

            // every expression tree which has data outside of it creates a hidden class 
            // with structure similar to 'SomeHiddenClass' (just an example) it is created to
            // save the reference so we when invoke or analize the id we can get the value
            #endregion

            var myClass = new MyClass();

            Func<MyClass, string> func = c => c.MyMethod(42, "test");
            Func<MyClass, bool> prop = c => c.MyProperty;

            var id = 42;
            Expression<Func<MyClass, string>> expr = c => c.MyMethod(42, "test");
            Expression<Func<MyClass, string>> expr2 = c => c.MyMethod(id, "test");
            Expression<Func<MyClass, bool>> propExpr = c => c.MyProperty;
            Expression<Func<MyClass, bool>> propExpr2 = c => c.MyProperty;
            Console.WriteLine(propExpr == propExpr2); // although the type and syntax is the same for C# these are different expressions
                                                      // we cannot cache expression tree directly in a dictionary

            Console.WriteLine(expr2.ToString());

            ParseExpression(expr);
            ParseExpression(propExpr);


            var exprFunc = expr.Compile(); // Slow! 

            var result = exprFunc(myClass);

            Console.WriteLine(result);

            // Example of creating expressions on the fly (advanced)
            var myClassType = typeof(MyClass);

            var numberConstant = Expression.Constant(42);
            var textConstant = Expression.Constant("My test expression");

            var parameterExpression = Expression.Parameter(myClassType, "c");

            var methodInfo = myClassType.GetMethod(nameof(MyClass.MyMethod));

            var callExpression = Expression.Call(parameterExpression, methodInfo, numberConstant, textConstant); 
            // if we are not creating a valid call we will have a lot of exceptions. we need a lot of validation checks.

            var lambdaExpression = Expression.Lambda<Func<MyClass, string>>(callExpression, parameterExpression); // creates - 'c => c.MyMethod(42, "test");'
        }

        private static void ParseExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
            {
                var lambdaExpr = (LambdaExpression)expression;
                Console.Write("Lambda: ");

                ParseExpression(lambdaExpr.Body);
                Console.WriteLine(lambdaExpr.Parameters[0].Name);
            }
            else if (expression.NodeType == ExpressionType.Call)
            {
                var methodCallExpr = (MethodCallExpression)expression;
                Console.Write("Method: ");

                Console.WriteLine(methodCallExpr.Method.Name); // return MyMethod from expr
                for (int i = 0; i < methodCallExpr.Arguments.Count; i++)
                {
                    ParseExpression(methodCallExpr.Arguments[i]); // Hits the ExpressionType.Constants
                }
            }
            else if(expression.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpr = (MemberExpression)expression;
                Console.Write("Member: ");
                Console.WriteLine(memberExpr.Member.Name); // return MyProperty from propExpr
            }
            else if(expression.NodeType == ExpressionType.Constant)
            {
                var constantExpr = (ConstantExpression)expression;
                Console.Write("Constant: ");
                Console.WriteLine(constantExpr.Value);
            }
        }
    }
}
