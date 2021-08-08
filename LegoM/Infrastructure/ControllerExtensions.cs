namespace LegoM.Infrastructure
{
    using Microsoft.AspNetCore.Mvc;
    using System;

    public static class ControllerExtensions
    {
        public static string GetControllerName(this Type controllerType)
            => controllerType.Name.Replace(nameof(Controller), string.Empty);
    }
}
