Imports DotNetNuke.Entities.Modules
Imports Connect.Libraries.UserManagement
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership
Imports Telerik.Web.UI
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Common.Utilities

Namespace Connect.Modules.UserManagement.AccountRegistration

    Partial Class View
        Inherits ConnectUsersModuleBase

        Implements IActionable

#Region "Event Handlers"


#Region "Event Handlers"

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            'DotNetNuke.Framework.AJAX.RegisterScriptManager()

            ProcessFormTemplate(plhRegister, GetTemplate(ModuleTheme, Constants.TemplateName_Form, CurrentLocale, False), Nothing)

            Dim btnUpdate As Button = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_UpdateButton), Button)
            If Not btnUpdate Is Nothing Then
                AddHandler btnUpdate.Click, AddressOf btnUpdate_Click
            End If

            Dim btnLogin As Button = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_LoginButton), Button)
            If Not btnLogin Is Nothing Then
                AddHandler btnLogin.Click, AddressOf btnLogin_Click
            End If

            Dim btnLostPassword As Button = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_LostPasswordButton), Button)
            If Not btnLostPassword Is Nothing Then
                AddHandler btnLostPassword.Click, AddressOf btnLostPassword_Click
            End If

        End Sub

        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Register()
        End Sub

        Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            HandleLogin()
        End Sub

        Protected Sub btnLostPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            HandleLostPassword()
        End Sub

#End Region

#Region "Private Methods"

        Private Sub HandleLostPassword()
            Response.Redirect(NavigateURL(TabId, "", "ctl=SendPassword"))
        End Sub

        Private Sub HandleLogin()

            Dim txtUsername As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_UsernameForLogin), TextBox)
            Dim txtPassword As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_PasswordForLogin), TextBox)
            Dim chkRemember As CheckBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_RememberForLogin), CheckBox)

            If Not txtUsername Is Nothing AndAlso Not txtPassword Is Nothing Then

                Dim loginStatus As DotNetNuke.Security.Membership.UserLoginStatus
                Dim objUser As UserInfo = UserController.ValidateUser(PortalId, txtUsername.Text, txtPassword.Text, "", PortalSettings.PortalName, Request.UserHostAddress, loginStatus)

                Select Case loginStatus

                    Case UserLoginStatus.LOGIN_FAILURE

                        lblError.Text = Localization.GetString("LOGIN_FAILURE", LocalResourceFile)
                        pnlError.Visible = True

                    Case UserLoginStatus.LOGIN_INSECUREADMINPASSWORD, UserLoginStatus.LOGIN_INSECUREHOSTPASSWORD, UserLoginStatus.LOGIN_SUPERUSER, UserLoginStatus.LOGIN_SUCCESS

                        Dim blnPersistent As Boolean = False
                        If Not chkRemember Is Nothing Then
                            blnPersistent = chkRemember.Checked
                        End If
                        UserController.UserLogin(PortalId, objUser, PortalSettings.PortalName, Request.UserHostAddress, blnPersistent)

                        If Not Request.QueryString("ReturnURL") Is Nothing Then
                            Response.Redirect(Server.UrlDecode(Request.QueryString("ReturnURL")), True)
                        End If

                        If RedirectAfterSubmit <> Null.NullInteger Then
                            Response.Redirect(NavigateURL(RedirectAfterSubmit))
                        End If

                        Response.Redirect(NavigateURL(PortalSettings.HomeTabId))

                    Case UserLoginStatus.LOGIN_USERLOCKEDOUT

                        lblError.Text = Localization.GetString("LOGIN_USERLOCKEDOUT", LocalResourceFile)
                        pnlError.Visible = True

                    Case UserLoginStatus.LOGIN_USERNOTAPPROVED

                        lblError.Text = Localization.GetString("LOGIN_USERNOTAPPROVED", LocalResourceFile)
                        pnlError.Visible = True

                End Select

            End If
        End Sub

        Private Sub Register()

            pnlSuccess.Visible = False
            pnlError.Visible = False

            Dim strMessages As New List(Of String)

            Dim blnUpdateUsername As Boolean = False
            Dim blnUpdateFirstname As Boolean = False
            Dim blnUpdateLastname As Boolean = False
            Dim blnUpdateDisplayname As Boolean = False
            Dim blnUpdatePassword As Boolean = False
            Dim blnUpdateEmail As Boolean = False

            Dim txtUsername As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_Username), TextBox)
            blnUpdateUsername = (Not txtUsername Is Nothing)

            If blnUpdateUsername Then
                If Not IsValidUserAttribute(Constants.User_Username, plhRegister) Then
                    strMessages.Add("Error_MissingUsername")
                    AddErrorIndicator(Constants.User_Username, plhRegister)
                Else
                    RemoveErrorIndicator(Constants.User_Username, plhRegister, True)
                End If
            End If

            Dim txtEmail As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_Email), TextBox)
            blnUpdateEmail = (Not txtEmail Is Nothing)

            If blnUpdateEmail Then
                If Not IsValidUserAttribute(Constants.User_Email, plhRegister) Then
                    strMessages.Add("Error_InvalidEmail")
                    AddErrorIndicator(Constants.User_Email, plhRegister)
                Else
                    RemoveErrorIndicator(Constants.User_Email, plhRegister, True)
                End If
            End If

            Dim txtPassword As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_Password1), TextBox)
            Dim txtPassword2 As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_Password2), TextBox)
            blnUpdatePassword = (Not txtPassword Is Nothing AndAlso Not txtPassword2 Is Nothing)

            If blnUpdatePassword Then
                If Not IsValidUserAttribute(Constants.User_Password1, plhRegister) Then
                    strMessages.Add("Error_MissingPassword1")
                    AddErrorIndicator(Constants.User_Password1, plhRegister)
                Else
                    RemoveErrorIndicator(Constants.User_Password1, plhRegister, True)
                End If
                If Not IsValidUserAttribute(Constants.User_Password2, plhRegister) Then
                    strMessages.Add("Error_MissingPassword2")
                    AddErrorIndicator(Constants.User_Password2, plhRegister)
                Else
                    RemoveErrorIndicator(Constants.User_Password2, plhRegister, True)
                End If
            End If

            Dim txtPasswordQuestion As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_PasswordQuestion), TextBox)
            Dim blnUpdatePasswordQuestion As Boolean = (Not txtPasswordQuestion Is Nothing)
            If blnUpdatePasswordQuestion Then
                If Not IsValidUserAttribute(Constants.User_PasswordQuestion, plhRegister) Then
                    strMessages.Add("Error_MissingPasswordQuestion")
                    AddErrorIndicator(Constants.User_PasswordQuestion, plhRegister)
                Else
                    RemoveErrorIndicator(Constants.User_PasswordQuestion, plhRegister, True)
                End If
            End If

            Dim txtPasswordAnswer As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_PasswordAnswer), TextBox)
            Dim blnUpdatePasswordAnswer As Boolean = (Not txtPasswordAnswer Is Nothing)
            If blnUpdatePasswordAnswer Then
                If Not IsValidUserAttribute(Constants.User_PasswordAnswer, plhRegister) Then
                    strMessages.Add("Error_MissingPasswordAnswer")
                    AddErrorIndicator(Constants.User_PasswordAnswer, plhRegister)
                Else
                    RemoveErrorIndicator(Constants.User_PasswordAnswer, plhRegister, True)
                End If
            End If


            Dim txtFirstName As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_Firstname), TextBox)
            blnUpdateFirstname = (Not txtFirstName Is Nothing)

            If blnUpdateFirstname Then
                If Not IsValidUserAttribute(Constants.User_Firstname, plhRegister) Then
                    strMessages.Add("Error_MissingFirstname")
                    AddErrorIndicator(Constants.User_Firstname, plhRegister)
                Else
                    RemoveErrorIndicator(Constants.User_Firstname, plhRegister, True)
                End If
            End If

            Dim txtLastName As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_Lastname), TextBox)
            blnUpdateLastname = (Not txtLastName Is Nothing)


            If (blnUpdateFirstname And blnUpdateLastname) Then
                If txtLastName.Text.ToLower.Trim = txtFirstName.Text.ToLower.Trim Then
                    strMessages.Add("Error_LastnameLikeFirstname")
                    AddErrorIndicator(Constants.User_Firstname, plhRegister)
                End If
            End If

            If blnUpdateLastname Then
                If Not IsValidUserAttribute(Constants.User_Lastname, plhRegister) Then
                    strMessages.Add("Error_MissingLastname")
                    AddErrorIndicator(Constants.User_Lastname, plhRegister)
                Else
                    RemoveErrorIndicator(Constants.User_Lastname, plhRegister, True)
                End If
            End If

            Dim txtDisplayName As TextBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_Displayname), TextBox)
            blnUpdateDisplayname = (Not txtDisplayName Is Nothing)

            If blnUpdateDisplayname Then
                If Not IsValidUserAttribute(Constants.User_Displayname, plhRegister) Then
                    strMessages.Add("Error_MissingDisplayName")
                    AddErrorIndicator(Constants.User_Displayname, plhRegister)
                Else
                    RemoveErrorIndicator(Constants.User_Displayname, plhRegister, True)
                End If
            End If

            Dim blnProfileErrorAdded As Boolean = False
            For Each itemProp As String In GetPropertiesFromTempate(GetTemplate(ModuleTheme, Constants.TemplateName_Form, CurrentLocale, False))
                Try
                    Dim prop As ProfilePropertyDefinition = ProfileController.GetPropertyDefinitionByName(PortalId, itemProp.Substring(2)) 'itemprop comes in the form U:Propertyname or P:Propertyname
                    If Not prop Is Nothing Then
                        If Not IsValidProperty(Nothing, prop, plhRegister) Then
                            If blnProfileErrorAdded = False Then
                                strMessages.Add("Error_MissingProfileField")
                                blnProfileErrorAdded = True
                            End If
                            AddErrorIndicator(prop.PropertyDefinitionId.ToString, plhRegister)
                        Else
                            RemoveErrorIndicator(prop.PropertyDefinitionId.ToString, plhRegister, prop.Required)
                        End If
                    End If
                Catch
                End Try
            Next

            If strMessages.Count > 0 Then
                Me.pnlError.Visible = True
                Me.lblError.Text = "<ul>"
                For Each strMessage As String In strMessages
                    lblError.Text += "<li>" & Localization.GetString(strMessage, LocalResourceFile) & "</li>"
                Next
                lblError.Text += "</ul>"
                Exit Sub
            End If


            Dim oUser As UserInfo = New UserInfo

            'approve membership if applicable
            If PortalSettings.UserRegistration = PortalRegistrationType.PublicRegistration Then
                oUser.Membership.Approved = True
            Else
                oUser.Membership.Approved = False
            End If

            'set defaults
            oUser.AffiliateID = Null.NullInteger
            oUser.PortalID = PortalSettings.PortalId

            oUser.IsDeleted = False
            oUser.IsSuperUser = False
            oUser.LastIPAddress = Request.UserHostAddress
            oUser.Profile = New UserProfile
            oUser.Username = ""
            oUser.DisplayName = ""
            oUser.Email = ""
            oUser.Membership.Password = ""



            'set username depending on setting
            Select Case UsernameMode
                Case UsernameUpdateMode.Email
                    If blnUpdateEmail Then
                        oUser.Username = txtEmail.Text.Trim
                    End If
                Case UsernameUpdateMode.FirstLetterLastname
                    If blnUpdateLastname AndAlso blnUpdateFirstname Then
                        oUser.Username = txtFirstName.Text.Trim.Substring(0, 1) & "." & txtLastName.Text
                    End If
                Case UsernameUpdateMode.FirstnameLastname
                    If blnUpdateLastname AndAlso blnUpdateFirstname Then
                        oUser.Username = txtFirstName.Text & "." & txtLastName.Text
                    End If
                Case UsernameUpdateMode.Lastname
                    If blnUpdateLastname Then
                        oUser.Username = txtLastName.Text
                    End If
                Case UsernameUpdateMode.UserSelect
                    If blnUpdateUsername Then
                        oUser.Username = txtUsername.Text
                    End If
            End Select

            'set displayname depending on setting
            Select Case DisplaynameMode
                Case DisplaynameUpdateMode.Email
                    If blnUpdateEmail Then
                        oUser.DisplayName = txtEmail.Text.Trim
                    End If
                Case DisplaynameUpdateMode.FirstLetterLastname
                    If blnUpdateLastname AndAlso blnUpdateFirstname Then
                        oUser.DisplayName = txtFirstName.Text.Trim.Substring(0, 1) & ". " & txtLastName.Text
                    End If
                Case DisplaynameUpdateMode.FirstnameLastname
                    If blnUpdateLastname AndAlso blnUpdateFirstname Then
                        oUser.DisplayName = txtFirstName.Text & " " & txtLastName.Text
                    Else

                    End If
                Case DisplaynameUpdateMode.Lastname
                    If blnUpdateLastname Then
                        oUser.DisplayName = txtLastName.Text
                    End If
                Case DisplaynameUpdateMode.UserSelect
                    If blnUpdateDisplayname Then
                        oUser.DisplayName = txtDisplayName.Text
                    End If
            End Select

            If blnUpdateEmail Then
                oUser.Email = txtEmail.Text
            End If

            'try updating password
            If blnUpdatePassword Then
                If txtPassword.Text = txtPassword2.Text Then
                    If UserController.ValidatePassword(txtPassword.Text) Then
                        oUser.Membership.Password = txtPassword.Text
                    Else

                        Dim MinLength As Integer = 0
                        Dim MinNonAlphaNumeric As Integer = 0
                        Try
                            MinLength = DotNetNuke.Security.Membership.MembershipProvider.Instance().MinPasswordLength
                        Catch
                        End Try
                        Try
                            MinNonAlphaNumeric = DotNetNuke.Security.Membership.MembershipProvider.Instance().MinNonAlphanumericCharacters
                        Catch
                        End Try

                        Dim strPolicy As String = String.Format(Localization.GetString("PasswordPolicy_MinLength", LocalResourceFile), MinLength.ToString)
                        If MinNonAlphaNumeric > 0 Then
                            strPolicy += String.Format(Localization.GetString("PasswordPolicy_MinNonAlphaNumeric", LocalResourceFile), MinNonAlphaNumeric.ToString)
                        End If

                        Me.pnlError.Visible = True
                        Me.lblError.Text = "<ul><li>" & String.Format(Localization.GetString("InvalidPassword", LocalResourceFile), strPolicy) & "</li></ul>"
                        Exit Sub
                    End If
                Else
                    Me.pnlError.Visible = True
                    Me.lblError.Text = "<ul><li>" & Localization.GetString("PasswordsDontMatch.Text", LocalResourceFile) & "</li></ul>"
                    Exit Sub
                End If
            Else
                'auto-generate password
                oUser.Membership.Password = UserController.GeneratePassword(7)
            End If

            If blnUpdatePasswordQuestion AndAlso blnUpdatePasswordAnswer Then
                oUser.Membership.PasswordQuestion = txtPasswordQuestion.Text
                oUser.Membership.PasswordAnswer = txtPasswordAnswer.Text
            End If

            If oUser.Username = "" Or oUser.Email = "" Or oUser.DisplayName = "" Or oUser.Membership.Password = "" Then
                'template must be setup up wrong
                Me.pnlError.Visible = True
                Me.lblError.Text = "<ul><li>" & String.Format(Localization.GetString("TemplateingError.Text", LocalResourceFile), PortalSettings.Email)

                If oUser.Username = "" Then
                    lblError.Text += " | Username is empty"
                End If

                If oUser.Email = "" Then
                    lblError.Text += " | E-mail is empty"
                End If

                If oUser.DisplayName = "" Then
                    lblError.Text += " | Displayname is empty"
                End If

                If oUser.Username = "" Then
                    lblError.Text += " | password is empty"
                End If

                lblError.Text += "</li></ul>"

                Exit Sub
            End If

            'set up profile
            oUser.Profile = New UserProfile
            oUser.Profile.InitialiseProfile(PortalSettings.PortalId, True)
            oUser.Profile.PreferredLocale = PortalSettings.DefaultLanguage
            oUser.Profile.PreferredTimeZone = PortalSettings.TimeZone
            'todo: create account
            Dim createStatus As UserCreateStatus = UserController.CreateUser(oUser)

            Dim strStatus As String = ""

            If createStatus <> UserCreateStatus.Success Then

                Select Case createStatus
                    Case UserCreateStatus.UsernameAlreadyExists

                        Select Case UsernameMode
                            Case UsernameUpdateMode.UserSelect
                                strStatus = Localization.GetString("UsernameAlreadyExists", Me.LocalResourceFile)
                            Case UsernameUpdateMode.Email
                                strStatus = Localization.GetString("DuplicateEmail", Me.LocalResourceFile)
                            Case UsernameUpdateMode.FirstnameLastname, UsernameUpdateMode.FirstLetterLastname, UsernameUpdateMode.Lastname
                                strStatus = Localization.GetString("NameAlreadyExists", Me.LocalResourceFile)
                        End Select

                    Case Else

                        strStatus = String.Format(Localization.GetString("CreateError", Me.LocalResourceFile), createStatus.ToString)

                End Select

                If strStatus <> "" Then
                    strStatus = "<li>" & strStatus & "</li>"
                Else
                    strStatus = "<li>" & createStatus.ToString & "</li>"
                End If

                Me.pnlError.Visible = True
                Me.lblError.Text = "<ul>" & strStatus & "</ul>"

                Exit Sub
            End If

            Dim propertiesCollection As New ProfilePropertyDefinitionCollection
            UpdateProfileProperties(plhRegister, oUser, propertiesCollection, GetPropertiesFromTempate(GetTemplate(ModuleTheme, Constants.TemplateName_Form, CurrentLocale, False)))
            oUser = ProfileController.UpdateUserProfile(oUser, propertiesCollection)

            If blnUpdateFirstname = True Then
                oUser.Profile.FirstName = txtFirstName.Text
                oUser.FirstName = txtFirstName.Text
            Else
                If oUser.Profile.FirstName <> "" Then
                    oUser.FirstName = oUser.Profile.FirstName
                End If
            End If

            If blnUpdateLastname = True Then
                oUser.Profile.LastName = txtLastName.Text
                oUser.LastName = txtLastName.Text
            Else
                If oUser.Profile.LastName <> "" Then
                    oUser.LastName = oUser.Profile.LastName
                End If
            End If

            If oUser.Username = "" Or oUser.Email = "" Or oUser.DisplayName = "" Or oUser.Membership.Password = "" Or oUser.FirstName = "" Or oUser.LastName = "" Then
                'template must be setup up wrong
                Me.pnlError.Visible = True
                Me.lblError.Text = "<ul><li>" & String.Format(Localization.GetString("TemplateingError.Text", LocalResourceFile), PortalSettings.Email)

                If oUser.Username = "" Then
                    lblError.Text += " | Username is empty"
                End If

                If oUser.Email = "" Then
                    lblError.Text += " | E-mail is empty"
                End If

                If oUser.DisplayName = "" Then
                    lblError.Text += " | Displayname is empty"
                End If

                If oUser.Username = "" Then
                    lblError.Text += " | password is empty"
                End If


                If oUser.FirstName = "" Then
                    lblError.Text += " | Firstname is empty"
                End If

                If oUser.LastName = "" Then
                    lblError.Text += " | LastName is empty"
                End If

                lblError.Text += "</li></ul>"

                Exit Sub
            End If

            Try
                oUser.Profile.SetProfileProperty("Email", oUser.Email)
            Catch
            End Try

            'update profile
            ProfileController.UpdateUserProfile(oUser)
            UserController.UpdateUser(PortalId, oUser)


            Dim strUserBody As String = ""
            Dim strAdminBody As String = ""


            If NotifyUser Then
                If PortalSettings.UserRegistration = PortalRegistrationType.PrivateRegistration Then
                    strUserBody = GetTemplate(ModuleTheme, Constants.TemplateName_EmailToUser_Private, CurrentLocale, False)
                ElseIf PortalSettings.UserRegistration = PortalRegistrationType.VerifiedRegistration Then
                    strUserBody = GetTemplate(ModuleTheme, Constants.TemplateName_EmailToUser_Verified, CurrentLocale, False)
                Else
                    strUserBody = GetTemplate(ModuleTheme, Constants.TemplateName_EmailToUser, CurrentLocale, False)
                End If
            End If

            If NotifyRole <> "" Then
                strAdminBody = GetTemplate(ModuleTheme, Constants.TemplateName_EmailToAdmin, CurrentLocale, False)
            End If

            If strAdminBody <> "" Then

                strAdminBody = strAdminBody.Replace("[PORTALURL]", PortalSettings.PortalAlias.HTTPAlias)
                strAdminBody = strAdminBody.Replace("[PORTALNAME]", PortalSettings.PortalName)
                strAdminBody = strAdminBody.Replace("[USERID]", oUser.UserID)
                strAdminBody = strAdminBody.Replace("[DISPLAYNAME]", oUser.DisplayName)

                If MembershipProvider.Instance().PasswordRetrievalEnabled Then
                    strAdminBody = strAdminBody.Replace("[PASSWORD]", MembershipProvider.Instance().GetPassword(oUser, ""))
                End If

                strAdminBody = strAdminBody.Replace("[USERNAME]", oUser.Username)
                strAdminBody = strAdminBody.Replace("[FIRSTNAME]", oUser.FirstName)
                strAdminBody = strAdminBody.Replace("[LASTNAME]", oUser.LastName)
                strAdminBody = strAdminBody.Replace("[EMAIL]", oUser.Email)

                If PortalSettings.UserRegistration = PortalRegistrationType.PrivateRegistration Then
                    strAdminBody = strAdminBody.Replace("[ADMINACTION]", Localization.GetString("AuthorizeAccount.Action", LocalResourceFile))
                    strAdminBody = strAdminBody.Replace("[REGISTRATIONMODE]", Localization.GetString("RegistrationMode_Private.Text", LocalResourceFile))
                ElseIf PortalSettings.UserRegistration = PortalRegistrationType.VerifiedRegistration Then
                    strAdminBody = strAdminBody.Replace("[ADMINACTION]", Localization.GetString("VerifyAccount.Action", LocalResourceFile))
                    strAdminBody = strAdminBody.Replace("[REGISTRATIONMODE]", Localization.GetString("RegistrationMode_Verified.Text", LocalResourceFile))
                Else
                    strAdminBody = strAdminBody.Replace("[ADMINACTION]", Localization.GetString("NoAction.Action", LocalResourceFile))
                    strAdminBody = strAdminBody.Replace("[REGISTRATIONMODE]", Localization.GetString("RegistrationMode_Public.Text", LocalResourceFile))
                End If

                strAdminBody = strAdminBody.Replace("[USERURL]", NavigateURL(UsermanagementTab, "", "uid=" & oUser.UserID.ToString))



                Dim ctrlRoles As New RoleController
                Dim NotificationUsers As ArrayList = ctrlRoles.GetUsersByRoleName(PortalId, NotifyRole)
                For Each NotificationUser As UserInfo In NotificationUsers
                    Try

                        strAdminBody = strAdminBody.Replace("[RECIPIENTUSERID]", NotificationUser.UserID.ToString)
                        strAdminBody = strAdminBody.Replace("[USERID]", NotificationUser.UserID.ToString)

                        DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, NotificationUser.Email, "", String.Format(Localization.GetString("NotifySubject_UserRegistered.Text", LocalResourceFile), PortalSettings.PortalName), strAdminBody, "", "HTML", "", "", "", "")
                    Catch
                    End Try
                Next

            End If

            If strUserBody <> "" Then

                strUserBody = strUserBody.Replace("[PORTALURL]", PortalSettings.PortalAlias.HTTPAlias)
                strUserBody = strUserBody.Replace("[PORTALNAME]", PortalSettings.PortalName)
                strUserBody = strUserBody.Replace("[USERID]", oUser.UserID)
                strUserBody = strUserBody.Replace("[DISPLAYNAME]", oUser.DisplayName)

                If MembershipProvider.Instance().PasswordRetrievalEnabled Then
                    strUserBody = strUserBody.Replace("[PASSWORD]", MembershipProvider.Instance().GetPassword(oUser, ""))
                End If

                strUserBody = strUserBody.Replace("[USERNAME]", oUser.Username)
                strUserBody = strUserBody.Replace("[FIRSTNAME]", oUser.FirstName)
                strUserBody = strUserBody.Replace("[LASTNAME]", oUser.LastName)
                strUserBody = strUserBody.Replace("[EMAIL]", oUser.Email)
                strUserBody = strUserBody.Replace("[VERIFICATIONCODE]", PortalSettings.PortalId.ToString & "-" & oUser.UserID.ToString)

                strUserBody = strUserBody.Replace("[RECIPIENTUSERID]", oUser.UserID.ToString)
                strUserBody = strUserBody.Replace("[USERID]", oUser.UserID.ToString)

                If PortalSettings.UserTabId <> Null.NullInteger Then
                    strUserBody = strUserBody.Replace("[USERURL]", NavigateURL(PortalSettings.UserTabId))
                Else
                    strUserBody = strUserBody.Replace("[USERURL]", NavigateURL(PortalSettings.HomeTabId, "ctl=Profile"))
                End If

                Dim returnurl As String = ""
                Dim loginurl As String = ""
                Dim verificationkey As String = PortalSettings.PortalId.ToString & "-" & oUser.UserID.ToString

                If PortalSettings.LoginTabId <> Null.NullInteger Then
                    loginurl = NavigateURL(PortalSettings.LoginTabId, "", "VerificationCode=" & verificationkey)
                Else
                    loginurl = NavigateURL(PortalSettings.HomeTabId, "", "ctl=Login", "VerificationCode=" & verificationkey)
                End If

                strUserBody = strUserBody.Replace("[VERIFYURL]", loginurl)

                Try
                    DotNetNuke.Services.Mail.Mail.SendMail(PortalSettings.Email, oUser.Email, "", String.Format(Localization.GetString("NotifySubject_UserDetails.Text", LocalResourceFile), PortalSettings.PortalName), strUserBody, "", "HTML", "", "", "", "")
                Catch
                End Try


            End If

            'add to role
            If AddToRoleOnSubmit <> Null.NullInteger Then
                Try
                    Dim rc As New RoleController
                    rc.AddUserRole(PortalId, oUser.UserID, AddToRoleOnSubmit, Null.NullDate)
                Catch
                End Try
            End If

            Dim blnAddMembership As Boolean = False

            Dim chkTest As CheckBox = CType(FindMembershipControlsRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_RoleMembership), CheckBox)
            If Not chkTest Is Nothing Then
                'at least on role membership checkbox found. Now lookup roles that could match
                Dim rc As New RoleController
                Dim roles As ArrayList
                roles = rc.GetPortalRoles(PortalId)
                For Each objRole As RoleInfo In roles

                    Dim blnPending As Boolean = False
                    Dim chkRole As CheckBox = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_RoleMembership & objRole.RoleName.Replace(" ", "")), CheckBox)
                    If chkRole Is Nothing Then
                        chkRole = CType(FindControlRecursive(plhRegister, plhRegister.ID & "_" & Constants.ControlId_RoleMembership & objRole.RoleName.Replace(" ", "") & "_Pending"), CheckBox)
                        blnPending = True
                    End If

                    If Not chkRole Is Nothing Then
                        If blnPending Then
                            rc.AddUserRole(PortalId, oUser.UserID, objRole.RoleID, RoleStatus.Pending, False, Date.Now, Null.NullDate)
                        Else
                            rc.AddUserRole(PortalId, oUser.UserID, objRole.RoleID, RoleStatus.Approved, False, Date.Now, Null.NullDate)
                        End If
                    End If

                Next
            End If

            If PortalSettings.UserRegistration = PortalRegistrationType.PublicRegistration Then

                'logon user
                Dim logonStatus As DotNetNuke.Security.Membership.UserLoginStatus = UserLoginStatus.LOGIN_FAILURE
                UserController.UserLogin(PortalId, oUser.Username, oUser.Membership.Password, "", PortalSettings.PortalName, Request.UserHostAddress, logonStatus, True)

                'see if all worked
                If logonStatus <> UserLoginStatus.LOGIN_SUCCESS Then

                    strStatus += logonStatus.ToString
                    Me.pnlError.Visible = True
                    Me.lblError.Text = strStatus
                    Exit Sub

                End If

            End If

            lblSucess.Text = "<ul><li>" & String.Format(Localization.GetString("RegisterSuccess", LocalResourceFile), NavigateURL(PortalSettings.HomeTabId)) & "</li></ul>"
            pnlSuccess.Visible = True


            If ExternalInterface <> Null.NullString Then

                Dim objInterface As Object = Nothing

                If ExternalInterface.Contains(",") Then
                    Dim strAssembly As String = ExternalInterface.Split(Char.Parse(","))(0).Trim
                    Dim strClass As String = ExternalInterface.Split(Char.Parse(","))(1).Trim
                    objInterface = System.Activator.CreateInstance(strAssembly, strClass).Unwrap
                End If

                If Not objInterface Is Nothing Then
                    CType(objInterface, Interfaces.iAccountRegistration).FinalizeAccountRegistration(Server, Response, Request, oUser)
                End If

            End If

            If Not Request.QueryString("ReturnURL") Is Nothing Then
                Response.Redirect(Server.UrlDecode(Request.QueryString("ReturnURL")), True)
            End If

            If RedirectAfterSubmit <> Null.NullInteger Then
                Response.Redirect(NavigateURL(RedirectAfterSubmit))
            End If

        End Sub

#End Region

        Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

            ManageRegionLabel(Me.plhRegister)

        End Sub

#End Region

#Region "Optional Interfaces"

        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
                Actions.Add(GetNextActionID, Localization.GetString("ManageTemplates.Action", LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl("ManageTemplates"), False, DotNetNuke.Security.SecurityAccessLevel.Edit, True, False)
                Return Actions
            End Get
        End Property

#End Region

    End Class

End Namespace


