namespace MesaDeAyuda.Data.Common.Helpers;

public static class RegularExpresions
{
    public const string Rut = @"^(0*(\d{1,3}(\.?\d{3})*)\-?([\dkK]))$";
    public const string Contrasenia =
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
}

public static class SystemConstants
{
    public const string DefaultAdminRut = "11111111-1";
    public const string DefaultAdminEmail = "admin@mesaayuda.com";
    public const string DefaultAdminNombre = "Administrador Sistema";
    public const string DefaultAdminContrasenia = "Admin123!";
}
