<%@ Control Language="vb" AutoEventWireup="false" Inherits="Connect.Modules.UserManagement.AccountRegistration.Settings" Codebehind="Settings.ascx.vb" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>


<div class="dnnFormItem">    
    <dnn:Label ID="lblUsernameMode" runat="server" Text="Username Mode:"></dnn:Label>
    <asp:DropDownList ID="drpUsernameMode" runat="server" AutoPostBack="false">
        <asp:ListItem Value="DISPLAY" Text="Yes" resourcekey="Username_FreeEnter"></asp:ListItem>
        <asp:ListItem Value="EMAIL" Text="No, use e-mail address" resourcekey="Username_UseEmail"></asp:ListItem>
        <asp:ListItem Value="FIRSTLETTER.LASTNAME" Text="No, use [Firstletter.Lastname]" resourcekey="Username_FirstletterLastname"></asp:ListItem>                
        <asp:ListItem Value="FIRSTNAME.LASTNAME" Text="No, use [Firstname.Lastname]" resourcekey="Username_FirstnameLastname"></asp:ListItem>                
        <asp:ListItem Value="LASTNAME" Text="No, use [Lastname]" resourcekey="Username_Lastname"></asp:ListItem>                
    </asp:DropDownList>    
</div>
<div class="dnnFormItem">
    <dnn:Label ID="lblDisplaynameMode" runat="server" Text="Displayname Mode:"></dnn:Label>
    <asp:DropDownList ID="drpDisplaynameMode" runat="server" AutoPostBack="false">
        <asp:ListItem Value="DISPLAY" Text="Yes" resourcekey="Displayname_FreeEnter"></asp:ListItem>
        <asp:ListItem Value="EMAIL" Text="No, use e-mail address" resourcekey="Displayname_FromEmail"></asp:ListItem>
        <asp:ListItem Value="FIRSTLETTER.LASTNAME" Text="No, use [Firstletter. Lastname]" resourcekey="Displayname_FirstletterLastname"></asp:ListItem>                
        <asp:ListItem Value="FIRSTNAME.LASTNAME" Text="No, use [Firstname Lastname]" resourcekey="Displayname_FirstnameLastname"></asp:ListItem>                
        <asp:ListItem Value="LASTNAME" Text="No, use [Lastname]" resourcekey="Displayname_Lastname"></asp:ListItem>                
    </asp:DropDownList>    
</div>

<div class="dnnFormItem">
    <dnn:Label ID="lblRedirectAfterSubmit" runat="server" Text="Redirect after submit:"></dnn:Label>
    <asp:DropDownList ID="drpRedirectAfterSubmit" runat="server" DataTextField="IndentedTabName" DataValueField="TabId" />
</div>


<div class="dnnFormItem">
    <dnn:Label ID="lblUsermanagementTab" runat="server" Text="Select Usermanagement Tab:"></dnn:Label> 
    <asp:DropDownList ID="drpUserManagementTab" runat="server" DataTextField="IndentedTabName" DataValueField="TabId" />    
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblAddToRole" runat="server" Text="Add to role on submit:"></dnn:Label>
     <asp:DropDownList ID="drpAddToRole" runat="server" DataTextField="RoleName" DataValueField="RoleId"></asp:DropDownList>
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblNotifyRole" runat="server" Text="Send confirmation e-mail to Role:"></dnn:Label>
     <asp:DropDownList ID="drpNotifyRole" runat="server" DataTextField="RoleName" DataValueField="RoleId"></asp:DropDownList>
</div>

<div class="dnnFormItem">
     <dnn:Label ID="lblNotifyUser" runat="server" Text="Send confirmation e-mail to user:"></dnn:Label>
     <asp:CheckBox ID="chkNotifyUser" runat="server" />
</div>