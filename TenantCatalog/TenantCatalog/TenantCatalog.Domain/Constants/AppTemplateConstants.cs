using System;

namespace TenantCatalog.Domain.Constants
{
    /// <summary>
    /// The authorizations constants.
    /// </summary>
    public static class AppTemplateConstants
    {
        /// <summary>
        /// The mileage document type.
        /// </summary>
        public const string MileageDocumentType = "mileage";

        /// <summary>
        /// The shift document type.
        /// </summary>
        public const string ShiftDocumentType = "shift";

        /// <summary>
        /// The ghs document type.
        /// </summary>
        public const string GhsDocumentType = "ghs";

        /// <summary>
        /// The gha document type.
        /// </summary>
        public const string GhaDocumentType = "gha";

        /// <summary>
        /// The enrichment data document type.
        /// </summary>
        public static readonly string EnrichmentDataDocumentType = "enrichmentdata";

        /// <summary>
        /// The external data document type.
        /// </summary>
        public static readonly string ExternalDataDocumentType = "externaldata";

        /// <summary>
        /// The transaction status document type.
        /// </summary>
        public static readonly string TransStatusDocumentType = "transstatus";

        /// <summary>
        /// The fee document type.
        /// </summary>
        public static readonly string FeeDocumentType = "fee";

        /// <summary>
        /// The outstanding time document type.
        /// </summary>
        public static readonly string OutstandingTimeDocumentType = "outstgtimsht";

        /// <summary>
        /// The outstanding mileage document type.
        /// </summary>
        public static readonly string OutstandingMileageDocumentType = "outstgmil";

        /// <summary>
        /// The participant mapping document type.
        /// </summary>
        public static readonly string ParticipantMappingDocumentType = "v4associatedentities";

        /// <summary>
        /// The default service code.
        /// </summary>
        public static readonly string DefaultServiceCode = "MISSING";

        /// <summary>
        /// The default caregiver id.
        /// </summary>
        public static readonly string DefaultCaregiverId = "0";

        /// <summary>
        /// The default participant id.
        /// </summary>
        public static readonly string DefaultParticipantId = "0";

        /// <summary>
        /// The min date.
        /// </summary>
        public static readonly DateTime MinDate = new DateTime(1990, 1, 1, 3, 0, 0);

        /// <summary>
        /// The shift record type.
        /// </summary>
        public static readonly string ShiftRecordType = "TCN";

        /// <summary>
        /// The shift sub type.
        /// </summary>
        public static readonly string ShiftSubType = "SHF";

        /// <summary>
        /// The mileage sub type.
        /// </summary>
        public static readonly string MileageSubType = "MLG";

        /// <summary>
        /// The gha sub type.
        /// </summary>
        public static readonly string GhaSubType = "GHA";

        /// <summary>
        /// The ghs sub type.
        /// </summary>
        public static readonly string GhsSubType = "GHS";

        /// <summary>
        /// The shift auto number format.
        /// </summary>
        public static readonly string ShiftAutoNumberFormat = "000000";

        /// <summary>
        /// The default priority.
        /// </summary>
        public static readonly int DefaultPriority = 99;

        /// <summary>
        /// The default record type.
        /// </summary>
        public static readonly string DefaultRecordType = "PR";

        /// <summary>
        /// The max caregiver participant id length.
        /// </summary>
        public static readonly int MaxCgPtIdLength = 15;

        /// <summary>
        /// The max unique id length.
        /// </summary>
        public static readonly int MaxUniqueIdLength = 32;

        /// <summary>
        /// The max comment length.
        /// </summary>
        public static readonly int MaxCommentLength = 500;

        /// <summary>
        /// The max company code length.
        /// </summary>
        public static readonly int MaxCompanyCodeLength = 5;

        /// <summary>
        /// The max misc property length.
        /// </summary>
        public static readonly int MaxMiscPropertyLength = 20;

        /// <summary>
        /// The company code.
        /// </summary>
        public static readonly string CompanyCode = "CompanyCode";

        /// <summary>
        /// The user id.
        /// </summary>
        public static readonly string UserId = "UserId";

        /// <summary>
        /// The json content type.
        /// </summary>
        public static readonly string JsonContentType = "application/json";

        /// <summary>
        /// The admin participant id string.
        /// </summary>
        public static readonly string AdminParticipantIdString = "p{0}";

        /// <summary>
        /// The azure tenant id key.
        /// </summary>
        public static readonly string AzureTenantIdKey = "AZURE_TENANT_ID";

        /// <summary>
        /// The ida tenant key.
        /// </summary>
        public static readonly string IdaTenantKey = "ida:Tenant";

        /// <summary>
        /// Gets the get all users screen permission.
        /// </summary>
        /// <value>
        /// The get all users screen permission.
        /// </value>
        public static readonly string GetAllUsersScreenPermission = "[dbo].[SPGetAllUsersScreenPermission]@Id, @CompanyId";

        /// <summary>
        /// Gets the All Companies.
        /// </summary>
        public static readonly string GetAllCompanies = "SpGetAllCompany @UserId";

        /// <summary>
        /// The unique identifier type.
        /// </summary>
        public static readonly string UserEmailType = "[dbo].[UserEmailUdt]";

        /// <summary>
        /// The All Companies.
        /// </summary>
        public static readonly string GetUserByEmail = "[dbo].[usp_GetUsersByEmail] @UserEmail";

        /// <summary>
        /// Gets the get user details.
        /// </summary>
        /// <value>
        /// The get user details.
        /// </value>
        public static readonly string GetUserDetails = "[dbo].[usp_GetUserDetails] @Id, @CompanyId";
    }
}
