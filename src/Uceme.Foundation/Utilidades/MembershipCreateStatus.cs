namespace Uceme.Foundation.Utilidades
{
    using System.Runtime.CompilerServices;

    //
    // Summary:
    //     Describes the result of a System.Web.Security.Membership.CreateUser(System.String,System.String)
    //     operation.
    [TypeForwardedFrom("System.Web, Version=2.0.0.0, Culture=Neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public enum MembershipCreateStatus
    {
        //
        // Summary:
        //     The user was successfully created.
        Success = 0,
        //
        // Summary:
        //     The user name was not found in the database.
        InvalidUserName = 1,
        //
        // Summary:
        //     The password is not formatted correctly.
        InvalidPassword = 2,
        //
        // Summary:
        //     The password question is not formatted correctly.
        InvalidQuestion = 3,
        //
        // Summary:
        //     The password answer is not formatted correctly.
        InvalidAnswer = 4,
        //
        // Summary:
        //     The e-mail address is not formatted correctly.
        InvalidEmail = 5,
        //
        // Summary:
        //     The user name already exists in the database for the application.
        DuplicateUserName = 6,
        //
        // Summary:
        //     The e-mail address already exists in the database for the application.
        DuplicateEmail = 7,
        //
        // Summary:
        //     The user was not created, for a reason defined by the provider.
        UserRejected = 8,
        //
        // Summary:
        //     The provider user key is of an invalid type or format.
        InvalidProviderUserKey = 9,
        //
        // Summary:
        //     The provider user key already exists in the database for the application.
        DuplicateProviderUserKey = 10,
        //
        // Summary:
        //     The provider returned an error that is not described by other System.Web.Security.MembershipCreateStatus
        //     enumeration values.
        ProviderError = 11,
    }
}
