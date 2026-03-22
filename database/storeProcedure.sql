--Procedimiento para listar los empleados ordenados por nombre
CREATE PROCEDURE dbo.listarEmpleados 
AS
BEGIN
    SELECT e.id, e.Nombre, e.Salario
    FROM dbo.Empleado e
    ORDER BY e.Nombre ASC;
END;
GO
--Procedimiento para insertar un nuevo empleado
CREATE OR ALTER PROCEDURE  dbo.insertarEmpleado 
    @inNombre VARCHAR(128),
    @inSalario MONEY,
    @outResultado INT OUTPUT
AS
BEGIN
    -- Validar si ya existe
    IF EXISTS (
        SELECT 1
        FROM dbo.Empleado e
        WHERE e.Nombre = @inNombre
    )
    BEGIN
        SET @outResultado = -1;
        RETURN;
    END

    -- Insertar empleado
    INSERT INTO dbo.Empleado (Nombre, Salario)
    VALUES (@inNombre, @inSalario);

    SET @outResultado = 1;
END;
GO