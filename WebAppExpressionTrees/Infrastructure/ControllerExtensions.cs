namespace WebAppExpressionTrees.Infrastructure
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;

    public static class ControllerExtensions
    {
        // e.g. in case we need caching for somethimg, we can use ConcurrentDictionary
        // request runs in parallel so if we dont use ConcurrentDictionary we will have race conditions;

        private static readonly ConcurrentDictionary<string, string> actionNameCache 
            = new ConcurrentDictionary<string, string>();

        public static IActionResult RedirectTo<TController>(
            this Controller controller,
            Expression<Action<TController>> redirectExpressions)
        {
            if (redirectExpressions.Body.NodeType != ExpressionType.Call)
            {
                throw new InvalidOperationException($"The provided expression is not valid method call: {redirectExpressions.Body}");
            }

            var methodCallExpr = redirectExpressions.Body as MethodCallExpression;

            var actionName = GetActionName(methodCallExpr);
            var controllerName = typeof(TController).Name.Replace(nameof(Controller), string.Empty);

            var routeValues = ExtractRootValues(methodCallExpr);

            return controller.RedirectToAction(actionName, controllerName, routeValues);
        }

        private static RouteValueDictionary ExtractRootValues(MethodCallExpression expression)
        {
            var names = expression.Method // ["id", "query"]
                .GetParameters()
                .Select(p => p.Name)
                .ToArray();

            var values = expression.Arguments
                .Select(arg =>
                {
                    if (arg.NodeType == ExpressionType.Constant)
                    {
                        var constantExpression = (ConstantExpression)arg;
                        return constantExpression.Value;
                    }

                    // easy way to extract values from variables
                    // for reference the advanced way is in the git repo
                    // https://github.com/ivaylokenov/MyTested.AspNetCore.Mvc/blob/development/src/MyTested.AspNetCore.Mvc.Abstractions/Utilities/ExpressionParser.cs 
                    // line 116

                    // () => arg -> Func<object>
                    // in order to work we need to cast it as an object
                    // () => (object)arg
                    var convertExpr = Expression.Convert(arg, typeof(object));
                    var funcExpr = Expression.Lambda<Func<object>>(convertExpr); 
                    return funcExpr.Compile().Invoke(); // Essentially create SomeFunc with lambda expression
                })
                .ToArray();

            var routeValueDictionary = new RouteValueDictionary();
            for (int i = 0; i < names.Length; i++)
            {
                routeValueDictionary.Add(names[i], values[i]);
            }

            return routeValueDictionary;
        }

        /// <summary>
        /// in case we have action name attribute used it will work with convetional routes and route attributes
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string GetActionName(MethodCallExpression expression)
        {
            // Example:
            // action name _ controller name
            // if we have route values we should include them in the cache as well
            var cacheKey = $"{expression.Method.Name}_{expression.Object.Type.Name}";

            return actionNameCache.GetOrAdd(cacheKey, _ =>
            {
                var methodName = expression.Method.Name;

                var actionNameAttribute = expression
                    .Method
                    .GetCustomAttributes(true)
                    .OfType<ActionNameAttribute>()
                    .FirstOrDefault()
                    ?.Name;

                return actionNameAttribute ?? methodName;

            });
        }

        private static object SomeFunc()
        {
            return 42;
        }
    }
}
