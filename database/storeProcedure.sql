-- Procedimiento para listar los empleados ordenados por nombre
CREATE OR ALTER PROCEDURE dbo.listarEmpleados
AS
BEGIN
    SET NOCOUNT ON;

    -- Selecciona los empleados con sus datos básicos ordenados alfabéticamente
    SELECT 
        e.id,
        e.nombre,
        e.salario
    FROM dbo.empleado e
    ORDER BY e.nombre ASC;

    SET NOCOUNT OFF;
END;
GO


-- Procedimiento para insertar un nuevo empleado
CREATE OR ALTER PROCEDURE dbo.insertarEmpleado
    @inNombre VARCHAR(128),
    @inSalario DECIMAL(10,2),
    @outResultado INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Verifica si ya existe un empleado con el mismo nombre
        IF EXISTS (
            SELECT 1
            FROM dbo.empleado e
            WHERE e.nombre = @inNombre
        )
        BEGIN
            SET @outResultado = -1; -- Error: empleado duplicado
            RETURN;
        END;

        -- Inserta un nuevo empleado en la tabla
        INSERT INTO dbo.empleado (
            nombre,
            salario
        )
        VALUES (
            @inNombre,
            @inSalario
        );

        SET @outResultado = 1; -- Inserción exitosa

    END TRY
    BEGIN CATCH

        -- Captura cualquier otro error ocurrido en ejecucion
        SET @outResultado = 0;

    END CATCH;

    SET NOCOUNT OFF;
END;
GO