<%@ Page Title="Login" Language="C#" MasterPageFile="~/Layouts/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KSAStaff._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="limiter">
        <div class="container-login100" style="background-color: #d9e1ef">
            <div class="wrap-login100">
                <div class="login100-form-title p-b-0 img-rounded">
                    <img src="../images/logo.png" alt="Logo" style="width:60%; height: auto;" />
                    <hr style="border: 5px solid #29AB87; border-radius: 2px" />
                </div>

                <div class="login100-form-title p-b-0">
                    <h5 style="font-family: jazz let"><u>KSA STAFF PORTAL</u></h5>
                </div>
                <asp:Panel DefaultButton="LbtnLogin" ID="LoginPanel" runat="server">
                    <asp:Label ID="lblError" runat="server" class="text-white text-center p-b-0 myglow"></asp:Label><br />
                    <br />
                    <div class="wrap-input100 validate-input" data-validate="Enter PF Number">
                        <input class="input100" type="text" runat="server" id="txtusername">
                        <span class="focus-input100" data-placeholder="PF Number"></span>
                    </div>

                    <div class="wrap-input100 validate-input" data-validate="Enter password">
                        <span class="btn-show-pass">
                            <i class="zmdi zmdi-eye"></i>
                        </span>
                        <input class="input100" type="password" runat="server" id="txtpassword">
                        <span class="focus-input100" data-placeholder="Password"></span>
                    </div>

                    <div class="container-login100-form-btn">
                        <div class="wrap-login100-form-btn" style="background-color: #29AB87">
                            <asp:LinkButton ID="lbtnLogin" type="submit" class="login100-form-btn" runat="server" OnClick="lbtnLogin_Click"><strong>LOGIN</strong></asp:LinkButton>
                        </div>
                    </div>

                    <div class="text-center p-t-15">

                        <asp:LinkButton ID="lbtnForgot" type="submit" CssClass="text-blue" runat="server" OnClick="lbtnForgot_Click">Forgot Password? <i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                    </div>
                </asp:Panel>

                <div class="text-center p-t-25">
                    <span class="txt1">
                        <strong>All Rights reserved KSA &copy; <%=DateTime.Now.Year %>. Powered by 
                            <a href="https://appkings.co.ke/" target="_blank">AppKings Solutions Ltd</a>. </strong>
                    </span>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
