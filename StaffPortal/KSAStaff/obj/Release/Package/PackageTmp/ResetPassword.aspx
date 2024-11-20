<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="KSAStaff.ResetPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="box box-danger box-solid" style="margin-top: 3rem;">
        <div class="box-header with-border">
            <h3 class="box-title"><i class="fa fa-lock"></i>Password Reset</h3>
            <div class="box-tools pull-right">
                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
            </div>
        </div>
        <div class="box-body">
            <p class="login-box-msg">Reset your password to continue</p>
            <div class="form-horizontal">
                <div class="box-body">
                    <asp:Label ID="lblError" runat="server" CssClass="label label-danger"></asp:Label>
                    <div class="form-group">
                        <label for="inputPassword3" class="col-sm-4 control-label">New Password</label>

                        <div class="col-sm-8">
                            <asp:TextBox ID="txtNewPass" class="form-control" placeholder="New Password" type="password" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputPassword3" class="col-sm-4 control-label">Confirm Password</label>

                        <div class="col-sm-8">
                            <asp:TextBox ID="txtConfirmpassword" class="form-control" placeholder="Confirm Password" type="password" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="box-footer">
                    <a id="lbtnForgot" href="Default.aspx" class="btn btn-warning"><i class="fa fa-home"></i>&nbsp;Home?</a>
                    <asp:LinkButton ID="lbtnResetPassword" type="submit" class="btn btn-danger pull-right" runat="server" OnClick="lbtnResetPassword_Click"><i class="fa fa-unlock"></i> Reset Password</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
