﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "AspNetRoles" (
        "Id" text NOT NULL,
        "Name" character varying(256) NULL,
        "NormalizedName" character varying(256) NULL,
        "ConcurrencyStamp" text NULL,
        CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "AspNetUsers" (
        "Id" text NOT NULL,
        "UserName" character varying(256) NULL,
        "NormalizedUserName" character varying(256) NULL,
        "Email" character varying(256) NULL,
        "NormalizedEmail" character varying(256) NULL,
        "EmailConfirmed" boolean NOT NULL,
        "PasswordHash" text NULL,
        "SecurityStamp" text NULL,
        "ConcurrencyStamp" text NULL,
        "PhoneNumber" text NULL,
        "PhoneNumberConfirmed" boolean NOT NULL,
        "TwoFactorEnabled" boolean NOT NULL,
        "LockoutEnd" timestamp with time zone NULL,
        "LockoutEnabled" boolean NOT NULL,
        "AccessFailedCount" integer NOT NULL,
        "FirstName" text NOT NULL,
        "SecondName" text NOT NULL,
        "Patronymic" text NULL,
        "CreatedAt" timestamp without time zone NOT NULL,
        CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "FieldsOfActivity" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        CONSTRAINT "PK_FieldsOfActivity" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "News" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Description" text NOT NULL,
        "Language" text NOT NULL,
        "CreatedAt" timestamp without time zone NOT NULL,
        "UserNameOwner" text NOT NULL,
        CONSTRAINT "PK_News" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "AspNetRoleClaims" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "RoleId" text NOT NULL,
        "ClaimType" text NULL,
        "ClaimValue" text NULL,
        CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "AspNetUserClaims" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "UserId" text NOT NULL,
        "ClaimType" text NULL,
        "ClaimValue" text NULL,
        CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "AspNetUserLogins" (
        "LoginProvider" character varying(128) NOT NULL,
        "ProviderKey" character varying(128) NOT NULL,
        "ProviderDisplayName" text NULL,
        "UserId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
        CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "AspNetUserRoles" (
        "UserId" text NOT NULL,
        "RoleId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
        CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "AspNetUserTokens" (
        "UserId" text NOT NULL,
        "LoginProvider" character varying(128) NOT NULL,
        "Name" character varying(128) NOT NULL,
        "Value" text NULL,
        CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
        CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "CommonExporters" (
        "UserId" text NOT NULL,
        "INN" character varying(12) NOT NULL,
        "OGRN_IP" character varying(15) NOT NULL,
        "LogoPath" text NULL,
        "FieldOfActivityId" integer NOT NULL,
        CONSTRAINT "PK_CommonExporters" PRIMARY KEY ("UserId"),
        CONSTRAINT "FK_CommonExporters_FieldsOfActivity_FieldOfActivityId" FOREIGN KEY ("FieldOfActivityId") REFERENCES "FieldsOfActivity" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_CommonExporters_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "LanguageExporters" (
        "CommonExporterId" text NOT NULL,
        "Language" text NOT NULL,
        "Name" text NOT NULL,
        "Description" text NULL,
        "ContactPersonFirstName" text NOT NULL,
        "ContactPersonSecondName" text NOT NULL,
        "ContactPersonPatronymic" text NULL,
        "DirectorFirstName" text NOT NULL,
        "DirectorSecondName" text NOT NULL,
        "DirectorPatronymic" text NULL,
        "WorkingTime" text NULL,
        "Address" text NULL,
        "Website" text NULL,
        "Approved" boolean NOT NULL,
        CONSTRAINT "PK_LanguageExporters" PRIMARY KEY ("CommonExporterId", "Language"),
        CONSTRAINT "FK_LanguageExporters_CommonExporters_CommonExporterId" FOREIGN KEY ("CommonExporterId") REFERENCES "CommonExporters" ("UserId") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE TABLE "Products" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Description" text NULL,
        "Language" text NOT NULL,
        "LanguageExporterId" text NOT NULL,
        "FieldOfActivityId" integer NOT NULL,
        "CreatedAt" timestamp without time zone NOT NULL,
        CONSTRAINT "PK_Products" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Products_FieldsOfActivity_FieldOfActivityId" FOREIGN KEY ("FieldOfActivityId") REFERENCES "FieldsOfActivity" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Products_LanguageExporters_LanguageExporterId_Language" FOREIGN KEY ("LanguageExporterId", "Language") REFERENCES "LanguageExporters" ("CommonExporterId", "Language") ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE INDEX "IX_CommonExporters_FieldOfActivityId" ON "CommonExporters" ("FieldOfActivityId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE INDEX "IX_Products_FieldOfActivityId" ON "Products" ("FieldOfActivityId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    CREATE INDEX "IX_Products_LanguageExporterId_Language" ON "Products" ("LanguageExporterId", "Language");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200716114442_Initial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200716114442_Initial', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200723113156_AddLogoToNews') THEN
    ALTER TABLE "News" ADD "Logo" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200723113156_AddLogoToNews') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200723113156_AddLogoToNews', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200727132013_CreateEvents') THEN
    CREATE TABLE "Events" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Description" text NULL,
        "Language" text NOT NULL,
        "CreatedAt" timestamp without time zone NOT NULL,
        "UserNameOwner" text NOT NULL,
        "Logo" text NULL,
        "StartsAt" timestamp without time zone NOT NULL,
        "EndsAt" timestamp without time zone NULL,
        CONSTRAINT "PK_Events" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200727132013_CreateEvents') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200727132013_CreateEvents', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200804085359_AddLogoToExporter') THEN
    ALTER TABLE "CommonExporters" DROP COLUMN "LogoPath";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200804085359_AddLogoToExporter') THEN
    ALTER TABLE "LanguageExporters" ADD "Logo" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200804085359_AddLogoToExporter') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200804085359_AddLogoToExporter', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200807133547_AddLogoToProduct') THEN
    ALTER TABLE "Products" ADD "Logo" text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200807133547_AddLogoToProduct') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200807133547_AddLogoToProduct', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200929145504_AddAboutRegionModel') THEN
    CREATE TABLE "AboutRegionContents" (
        "Lang" text NOT NULL,
        "Content" text NOT NULL,
        CONSTRAINT "PK_AboutRegionContents" PRIMARY KEY ("Lang")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200929145504_AddAboutRegionModel') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200929145504_AddAboutRegionModel', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20200930073446_EditAboutRegionModel') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20200930073446_EditAboutRegionModel', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201013150420_AddPhoneAndPositionToExporter') THEN
    ALTER TABLE "LanguageExporters" ALTER COLUMN "DirectorSecondName" TYPE text;
    ALTER TABLE "LanguageExporters" ALTER COLUMN "DirectorSecondName" DROP NOT NULL;
    ALTER TABLE "LanguageExporters" ALTER COLUMN "DirectorSecondName" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201013150420_AddPhoneAndPositionToExporter') THEN
    ALTER TABLE "LanguageExporters" ALTER COLUMN "DirectorFirstName" TYPE text;
    ALTER TABLE "LanguageExporters" ALTER COLUMN "DirectorFirstName" DROP NOT NULL;
    ALTER TABLE "LanguageExporters" ALTER COLUMN "DirectorFirstName" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201013150420_AddPhoneAndPositionToExporter') THEN
    ALTER TABLE "LanguageExporters" ADD "Phone" text NOT NULL DEFAULT '';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201013150420_AddPhoneAndPositionToExporter') THEN
    ALTER TABLE "LanguageExporters" ADD "Position" text NOT NULL DEFAULT '';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201013150420_AddPhoneAndPositionToExporter') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201013150420_AddPhoneAndPositionToExporter', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    ALTER TABLE "CommonExporters" DROP CONSTRAINT "FK_CommonExporters_FieldsOfActivity_FieldOfActivityId";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    ALTER TABLE "Products" DROP CONSTRAINT "FK_Products_FieldsOfActivity_FieldOfActivityId";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    DROP TABLE "FieldsOfActivity";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    DROP INDEX "IX_Products_FieldOfActivityId";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    DROP INDEX "IX_CommonExporters_FieldOfActivityId";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    ALTER TABLE "Products" DROP COLUMN "FieldOfActivityId";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    ALTER TABLE "CommonExporters" DROP COLUMN "FieldOfActivityId";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    ALTER TABLE "Products" ADD "IndustryId" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    ALTER TABLE "CommonExporters" ADD "IndustryId" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    CREATE TABLE "Industries" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        CONSTRAINT "PK_Industries" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    CREATE TABLE "IndustryTranslations" (
        "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Description" text NOT NULL,
        "Image" text NOT NULL,
        "Language" text NOT NULL,
        "IndustryId" integer NOT NULL,
        CONSTRAINT "PK_IndustryTranslations" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_IndustryTranslations_Industries_IndustryId" FOREIGN KEY ("IndustryId") REFERENCES "Industries" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    CREATE INDEX "IX_Products_IndustryId" ON "Products" ("IndustryId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    CREATE INDEX "IX_CommonExporters_IndustryId" ON "CommonExporters" ("IndustryId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    CREATE INDEX "IX_IndustryTranslations_IndustryId" ON "IndustryTranslations" ("IndustryId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    ALTER TABLE "CommonExporters" ADD CONSTRAINT "FK_CommonExporters_Industries_IndustryId" FOREIGN KEY ("IndustryId") REFERENCES "Industries" ("Id") ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    ALTER TABLE "Products" ADD CONSTRAINT "FK_Products_Industries_IndustryId" FOREIGN KEY ("IndustryId") REFERENCES "Industries" ("Id") ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201026145957_ReplaceFieldOfActivityWithIndustry') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201026145957_ReplaceFieldOfActivityWithIndustry', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201027153456_SetIndustryImagePropertyNullable') THEN
    ALTER TABLE "IndustryTranslations" ALTER COLUMN "Image" TYPE text;
    ALTER TABLE "IndustryTranslations" ALTER COLUMN "Image" DROP NOT NULL;
    ALTER TABLE "IndustryTranslations" ALTER COLUMN "Image" DROP DEFAULT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201027153456_SetIndustryImagePropertyNullable') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201027153456_SetIndustryImagePropertyNullable', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201112121840_AddStatusForExporter') THEN
    ALTER TABLE "LanguageExporters" DROP COLUMN "Approved";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201112121840_AddStatusForExporter') THEN
    ALTER TABLE "CommonExporters" ADD "IsShowedOnIndustryPage" boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201112121840_AddStatusForExporter') THEN
    ALTER TABLE "CommonExporters" ADD "Status" text NOT NULL DEFAULT '';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201112121840_AddStatusForExporter') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201112121840_AddStatusForExporter', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201123115056_AddOrderFieldToIndustryTranslation') THEN
    ALTER TABLE "IndustryTranslations" ADD "Order" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201123115056_AddOrderFieldToIndustryTranslation') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201123115056_AddOrderFieldToIndustryTranslation', '3.1.9');
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201123123303_AddOrderFieldToIndustry') THEN
    ALTER TABLE "IndustryTranslations" DROP COLUMN "Order";
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201123123303_AddOrderFieldToIndustry') THEN
    ALTER TABLE "Industries" ADD "Order" integer NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20201123123303_AddOrderFieldToIndustry') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20201123123303_AddOrderFieldToIndustry', '3.1.9');
    END IF;
END $$;
