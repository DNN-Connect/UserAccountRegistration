
Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Common.Utilities

Namespace Connect.Modules.UserManagement.AccountRegistration
    Partial Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"

        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then

                    BindPages()
                    BindRoles()

                    If (Settings.Contains("ExternalInterface")) Then txtInterface.Text = Settings("ExternalInterface").ToString()
                    If (Settings.Contains("ShowUserName")) Then drpUsernameMode.SelectedValue = Settings("ShowUserName").ToString()
                    If (Settings.Contains("ShowDisplayName")) Then drpDisplaynameMode.SelectedValue = Settings("ShowDisplayName").ToString()
                    If (Settings.Contains("RedirectAfterSubmit")) Then drpRedirectAfterSubmit.SelectedValue = Settings("RedirectAfterSubmit").ToString()
                    If (Settings.Contains("UsermanagementTab")) Then drpUserManagementTab.SelectedValue = Settings("UsermanagementTab").ToString()
                    If (Settings.Contains("AddToRoleOnSubmit")) Then drpAddToRole.SelectedValue = Settings("AddToRoleOnSubmit").ToString()
                    If (Settings.Contains("NotifyRole")) Then drpNotifyRole.Items.FindByText(Settings("NotifyRole").ToString()).Selected = True
                    If (Settings.Contains("NotifyUser")) Then chkNotifyUser.Checked = CType(Settings("NotifyUser"), Boolean)

                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New Entities.Modules.ModuleController

                objModules.UpdateTabModuleSetting(TabModuleId, "ExternalInterface", txtInterface.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "ShowUserName", drpUsernameMode.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "ShowDisplayName", drpDisplaynameMode.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "RedirectAfterSubmit", drpRedirectAfterSubmit.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "UsermanagementTab", drpUserManagementTab.SelectedValue)
                objModules.UpdateTabModuleSetting(TabModuleId, "AddToRoleOnSubmit", drpAddToRole.SelectedValue)
                'we need the rolename for sending mails to users, therefor store here the rolename rather than the id!
                objModules.UpdateTabModuleSetting(TabModuleId, "NotifyRole", drpNotifyRole.SelectedItem.Text)
                objModules.UpdateTabModuleSetting(TabModuleId, "NotifyUser", chkNotifyUser.Checked.ToString)

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub BindPages()

            Dim tabs As System.Collections.Generic.List(Of Entities.Tabs.TabInfo) = TabController.GetPortalTabs(PortalId, Null.NullInteger, True, True, False, False)

            drpRedirectAfterSubmit.DataSource = tabs
            drpRedirectAfterSubmit.DataBind()

            drpUserManagementTab.DataSource = tabs
            drpUserManagementTab.DataBind()

        End Sub

        Private Sub BindRoles()

            Dim rc As New Security.Roles.RoleController
            Dim roles As ArrayList = rc.GetPortalRoles(PortalId)

            drpAddToRole.DataSource = roles
            drpAddToRole.DataBind()
            drpAddToRole.Items.Insert(0, New ListItem("---", "-1"))

            drpNotifyRole.DataSource = roles
            drpNotifyRole.DataBind()
            drpNotifyRole.Items.Insert(0, New ListItem("---", "-1"))

        End Sub


#End Region

    End Class
End Namespace


