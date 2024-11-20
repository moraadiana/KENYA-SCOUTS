<%@ Page Title="Leave Application" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="LeaveApplication.aspx.cs" Inherits="KSAStaff.pages.LeaveApplication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
     <div class="content-wrapper">
        <section class="content-header">
            <h1>Leave Application</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Leave Application</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-diamond"></i>New Leave Application</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Leave Type</label>
                                        <asp:DropDownList ID="ddlLeaveType" runat="server" CssClass="form-control select2" OnSelectedIndexChanged="ddlLeaveType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Reliever</label>
                                        <asp:DropDownList ID="ddlReliver" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                    </div>
                                </div>
                               <%--  <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Responsibility Canter</label>
                                        <asp:DropDownList ID="ddlResponsibilityCenter" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                    </div>
                                </div> --%>
                                  <div class="col-md-3">
                                      <div class="form-group">
                                          <label>Applied Days</label>
                                          <asp:TextBox ID="txtAppliedDays" CssClass="form-control" runat="server"></asp:TextBox>
                                      </div>
                                  </div>
                                <div class="col-md-3">
                                     <div class="form-group">
                                         <label>Responsibility Center: </label>
                                         <asp:Label ID="lblResCenter" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                     </div>
                                </div>
                              
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Start Date</label>
                                        <asp:TextBox ID="txtStartDate" CssClass="form-control" runat="server" Widt="350px" TextMode="Date" OnTextChanged="txtStartDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <script>
                                            $j('#Main1_txtStartDate').Zebra_DatePicker({
                                                direction: [1, false],
                                                onSelect: function () {
                                                    this.trigger("change")
                                                }
                                            });</script>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Balance: </label>
                                        <asp:Label ID="lblBalance" runat="server" Text="" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                    </div>
                                </div>
                                <%--<div class="col-md-3">
                                    <div class="form-group">
                                        <label>KSA: </label>
                                        <asp:Label ID="lblDirectorate" runat="server" Text="" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                    </div>
                                </div>--%>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Department: </label>
                                        <asp:Label ID="lblDepartment" runat="server" Text="" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>End Date: </label>
                                        <asp:Label ID="lblEndDate" runat="server" Text="" Font-Bold="True" ForeColor="#FF6600" CssClass="spLabel"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Return Date: </label>
                                        <asp:Label ID="lblReturnDate" runat="server" Text="" Font-Bold="True" ForeColor="#FF6600" CssClass="spLabel"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Purpose</label>
                                        <asp:TextBox ID="txtPurpose" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <a href="LeaveListing.aspx" class="btn btn-warning pull-left"><i class="fa fa-backward"></i>&nbsp;Back</a>
                                    <asp:LinkButton ID="lbtnSubmit" CssClass="btn btn-success pull-right" runat="server" OnClick="lbtnSubmit_Click"><i class="fa fa-paper-plane"></i>&nbsp;Submit</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
