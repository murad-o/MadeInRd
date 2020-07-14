using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace ExporterWeb.Areas.Identity.Authorization
{
    public static class AuthorizationOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
    }

    static class Constants
    {
        public const string CreateOperationName = "Create";
        public const string ReadOperationName = "Read";
        public const string UpdateOperationName = "Update";
        public const string DeleteOperationName = "Delete";

        public const string ManagersRole = "ExporterManagers";
        public const string AdministratorsRole = "ExporterAdministrators";
        public const string ExportersRole = "Exporters";
        public const string AnalystsRole = "Analysts";
    }
}
