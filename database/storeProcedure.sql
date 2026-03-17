CREATE PROCEDURE dbo.listarEmpleadosid AS
BEGIN
    SELECT id, Nombre, Salario
    FROM dbo.Empleado
    ORDER BY id ;
END;
GO

CREATE PROCEDURE dbo.insertarEmpleado
    @Nombre VARCHAR(128),
    @Salario MONEY
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM dbo.Empleado
        WHERE Nombre = @Nombre
    )
    BEGIN
        PRINT 'El empleado ya existe';
        RETURN;
    END

    INSERT INTO dbo.Empleado (Nombre, Salario)
    VALUES (@Nombre, @Salario);
END;