namespace Uceme.UI.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ServicioCirugia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            /* migrationBuilder.DropForeignKey(
                name: "FK_Cita_Turno_TurnoidTurno",
                table: "Cita");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemCurriculum_Curriculum_CurriculumidCurriculum",
                table: "ItemCurriculum");

            migrationBuilder.DropForeignKey(
                name: "FK_Turno_DatosProfesionales_DatosProfesionalesidDatosPro",
                table: "Turno");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Curriculum_CurriculumidCurriculum",
                table: "Usuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_DatosContacto_DatosContactoidDatosContacto",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_CurriculumidCurriculum",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_DatosContactoidDatosContacto",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Turno_DatosProfesionalesidDatosPro",
                table: "Turno");

            migrationBuilder.DropIndex(
                name: "IX_ItemCurriculum_CurriculumidCurriculum",
                table: "ItemCurriculum");

            migrationBuilder.DropIndex(
                name: "IX_Cita_TurnoidTurno",
                table: "Cita");

            migrationBuilder.DropColumn(
                name: "CurriculumidCurriculum",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "DatosContactoidDatosContacto",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "DatosProfesionalesidDatosPro",
                table: "Turno");

            migrationBuilder.DropColumn(
                name: "CurriculumidCurriculum",
                table: "ItemCurriculum");

            migrationBuilder.DropColumn(
                name: "TurnoidTurno",
                table: "Cita");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsumedTime",
                table: "PersistedGrants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PersistedGrants",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "PersistedGrants",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DeviceCodes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "DeviceCodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_SessionId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "SessionId", "Type" }); */

            migrationBuilder.Sql("update [UCEMEDb].[dbo].[DatosProfesionales] set nombre = 'Endocrinología' where idDatosPro = 1");
            migrationBuilder.Sql("insert [UCEMEDb].[dbo].[DatosProfesionales] ([nombre],[telefono],[email],[direccion],[texto],[foto],[activo]) values ('Cirugía','[telefono]','[email]','[direccion]','[texto]','[foto]',1)");
            migrationBuilder.Sql("update [UCEMEDb].[dbo].[Turno] set idHospital = 3 where idTurno = 7");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /* migrationBuilder.DropIndex(
                name: "IX_PersistedGrants_SubjectId_SessionId_Type",
                table: "PersistedGrants");

            migrationBuilder.DropColumn(
                name: "ConsumedTime",
                table: "PersistedGrants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PersistedGrants");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "PersistedGrants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DeviceCodes");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "DeviceCodes");

            migrationBuilder.AddColumn<int>(
                name: "CurriculumidCurriculum",
                table: "Usuario",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DatosContactoidDatosContacto",
                table: "Usuario",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DatosProfesionalesidDatosPro",
                table: "Turno",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurriculumidCurriculum",
                table: "ItemCurriculum",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TurnoidTurno",
                table: "Cita",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_CurriculumidCurriculum",
                table: "Usuario",
                column: "CurriculumidCurriculum");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_DatosContactoidDatosContacto",
                table: "Usuario",
                column: "DatosContactoidDatosContacto");

            migrationBuilder.CreateIndex(
                name: "IX_Turno_DatosProfesionalesidDatosPro",
                table: "Turno",
                column: "DatosProfesionalesidDatosPro");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCurriculum_CurriculumidCurriculum",
                table: "ItemCurriculum",
                column: "CurriculumidCurriculum");

            migrationBuilder.CreateIndex(
                name: "IX_Cita_TurnoidTurno",
                table: "Cita",
                column: "TurnoidTurno");

            migrationBuilder.AddForeignKey(
                name: "FK_Cita_Turno_TurnoidTurno",
                table: "Cita",
                column: "TurnoidTurno",
                principalTable: "Turno",
                principalColumn: "idTurno",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCurriculum_Curriculum_CurriculumidCurriculum",
                table: "ItemCurriculum",
                column: "CurriculumidCurriculum",
                principalTable: "Curriculum",
                principalColumn: "idCurriculum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Turno_DatosProfesionales_DatosProfesionalesidDatosPro",
                table: "Turno",
                column: "DatosProfesionalesidDatosPro",
                principalTable: "DatosProfesionales",
                principalColumn: "idDatosPro",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Curriculum_CurriculumidCurriculum",
                table: "Usuario",
                column: "CurriculumidCurriculum",
                principalTable: "Curriculum",
                principalColumn: "idCurriculum",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_DatosContacto_DatosContactoidDatosContacto",
                table: "Usuario",
                column: "DatosContactoidDatosContacto",
                principalTable: "DatosContacto",
                principalColumn: "idDatosContacto",
                onDelete: ReferentialAction.Restrict); */
        }
    }
}
