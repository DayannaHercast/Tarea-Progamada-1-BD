--Conteo totalde empleados
SELECT COUNT(*) AS TotalEmpleados FROM dbo.empleado;
--Listado completo de empleados
SELECT id, Nombre, Salario FROM dbo.Empleado ORDER BY id;
--Orden alfabético
SELECT id, Nombre, Salario FROM dbo.Empleado ORDER BY Nombre ASC;
--Filtro por salario mayor a 600000
SELECT id, Nombre, Salario FROM dbo.Empleado WHERE Salario > 600000 ORDER BY Salario DESC;
