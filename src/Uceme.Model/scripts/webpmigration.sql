BEGIN TRANSACTION;
GO

update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog34.webp'
WHERE idBlog=34
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4057.webp'
WHERE idBlog=4057
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4055.webp'
WHERE idBlog=4055
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4054.webp'
WHERE idBlog=4054
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4054.webp'
WHERE idBlog=4054
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4052.webp'
WHERE idBlog=4052
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4051.webp'
WHERE idBlog=4051
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4047.webp'
WHERE idBlog=4047
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4045.webp'
WHERE idBlog=4045
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4044.webp'
WHERE idBlog=4044
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4040.webp'
WHERE idBlog=4040
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4035.webp'
WHERE idBlog=4035
update [UCEMEDb].[dbo].[Blog]
set foto = '~/uploads/fotos/Blog4030.webp'
WHERE idBlog=4030
GO

update [UCEMEDb].[dbo].[Companias]
set foto = '~/uploads/fotos/comp2.webp'
where idCompanias = 2
update [UCEMEDb].[dbo].[Companias]
set foto = '~/uploads/fotos/comp4.webp'
where idCompanias = 4
update [UCEMEDb].[dbo].[Companias]
set foto = '~/uploads/fotos/comp1011.webp'
where idCompanias = 1011
GO

UPDATE [UCEMEDb].[dbo].[DatosProfesionales]
set foto = '~/uploads/fotos/hosp1.webp'
where idDatosPro = 1UPDATE [UCEMEDb].[dbo].[DatosProfesionales]
set foto = '~/uploads/fotos/hosp2.webp'
where idDatosPro = 2
GO

UPDATE [UCEMEDb].[dbo].[Usuario]
SET foto='~/uploads/fotos/usu4.webp'
where idUsuario=4
UPDATE [UCEMEDb].[dbo].[Usuario]
SET foto='~/uploads/fotos/usu5.webp'
where idUsuario=5
UPDATE [UCEMEDb].[dbo].[Usuario]
SET foto='~/uploads/fotos/usu6.webp'
where idUsuario=6
UPDATE [UCEMEDb].[dbo].[Usuario]
SET foto='~/uploads/fotos/usu9.webp'
where idUsuario=9
UPDATE [UCEMEDb].[dbo].[Usuario]
SET foto='~/uploads/fotos/usu11.webp'
where idUsuario=11
GO

UPDATE [UCEMEDb].[dbo].[Tecnica]
set foto='~/uploads/fotos/Tecnica1.webp'
where idTecnica=1
UPDATE [UCEMEDb].[dbo].[Tecnica]
set foto='~/uploads/fotos/Tecnica2.webp'
where idTecnica=2
UPDATE [UCEMEDb].[dbo].[Tecnica]
set foto='~/uploads/fotos/Tecnica3.webp'
where idTecnica=3
GO

update [UCEMEDb].[dbo].[Servicio]
set foto = '~/uploads/fotos/ser1.webp'
WHERE idServicio=1
update [UCEMEDb].[dbo].[Servicio]
set foto = '~/uploads/fotos/ser2.webp'
WHERE idServicio=2
update [UCEMEDb].[dbo].[Servicio]
set foto = '~/uploads/fotos/ser6.webp'
WHERE idServicio=6
update [UCEMEDb].[dbo].[Servicio]
set foto = '~/uploads/fotos/ser8.webp'
WHERE idServicio=8
update [UCEMEDb].[dbo].[Servicio]
set foto = '~/uploads/fotos/ser11.webp'
WHERE idServicio=11
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230525192658_WebpMigration', N'7.0.6');
GO

COMMIT;
GO
