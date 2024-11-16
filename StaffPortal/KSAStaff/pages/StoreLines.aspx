<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="StoreLines.aspx.cs" Inherits="KSAStaff.pages.StoreLines" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
     <div class="content-wrapper">
        <section class="content-header">
            <h1>Store Requests</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Store Requests</li>
            </ol>
        </section>

        <section class="content">
            <div class="row">
            <div class="row">
                <div class="col-md-12">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="vwHeader" runat="server">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title"><i class="fa fa-diamond"></i>&nbsp;Store Request</h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                        <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-remove"></i></button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>User Id: </label>
                                                <asp:Label ID="lblUserId" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Staff Name: </label>
                                                <asp:Label ID="lblStaffName" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Directorate: </label>
                                                <asp:Label ID="lblDirectorate" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Department: </label>
                                                <asp:Label ID="lblDepartment" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Requisition Type</label>
                                                <asp:DropDownList ID="ddlRequisitionType" CssClass="form-control" runat="server">
                                                    <asp:ListItem Value="0">Stationery</asp:ListItem>
                                                    <asp:ListItem Value="1">Grocery</asp:ListItem>
                                                    <asp:ListItem Value="2">Project</asp:ListItem>
                                                    <asp:ListItem Value="3">Cleaning</asp:ListItem>
                                                    <asp:ListItem Value="4">Hardware</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Required Date </label>
                                                <asp:TextBox ID="txtRequiredDate" CssClass="form-control" runat="server" Widt="350px" TextMode="Date"></asp:TextBox>
                                                <script>
                                                    $j('#Main1_txtRequiredDate').Zebra_DatePicker({
                                                        direction: [1, false]
                                                    });</script>
                                            </div>
                                        </div>
                                         <div class="col-md-3">
                                                  <div class="form-group">
                                                      <label>Responsibility Center: </label>
                                                      <asp:Label ID="lblResCenter" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                                  </div>
                                        </div>

                                       <%-- <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Responsibility Center</label>
                                                <asp:DropDownList ID="ddlResponsibilityCenter" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>--%>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label>Issuing Store</label>
                                                <asp:DropDownList ID="ddlissuingStore" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <label>Description</label>
                                                <asp:TextBox ID="txtDescription" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:LinkButton ID="lbtnSubmit" CssClass="btn btn-success pull-right" OnClick="lbtnSubmit_Click" runat="server"><i class="fa fa-paper-plane"></i>&nbsp;Submit</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:View>
                        <asp:View ID="vwLines" runat="server">
                            <div class="box box-info">
                                <div class="box-header with-border">
                                    <h3 class="box-title"><i class="fa fa-diamond"></i>&nbsp;Store Lines -
                                        <asp:Label ID="lblLStoreNo" runat="server" Text=""></asp:Label></h3>
                                    <div class="box-tools pull-right">
                                        <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                        <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-remove"></i></button>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:LinkButton ID="lbtnAddLines" runat="server" CssClass="text-info pull-right" OnClick="lbtnAddLines_Click"><i class="fa fa-plus-circle"></i>&nbsp;Add Lines</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnCloseLines" runat="server" CssClass="text-danger pull-right" OnClick="lbtnCloseLines_Click"><i class="fa fa-minus-circle"></i>&nbsp;Close Lines</asp:LinkButton>
                                        </div>
                                    </div>
                                    <br />
                                    <div id="newLines" runat="server" visible="false">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Store No</label><br />
                                                    <asp:Label ID="lblStoreNo" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Type</label>
                                                    <asp:DropDownList ID="ddlType" CssClass="form-control select2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                                        <asp:ListItem Value="1">Item</asp:ListItem>
                                                        <asp:ListItem Value="2">Minor Asset</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Item</label>
                                                    <asp:DropDownList ID="ddlItem" CssClass="form-control select2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Issuing Store</label>
                                                    <asp:DropDownList ID="ddlIssuingStoreLines" CssClass="form-control select2" runat="server"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Quantity In Store</label>
                                                    <asp:TextBox ID="txtQtyInStore" CssClass="form-control" Enabled="false" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Quantity</label>
                                                    <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:LinkButton ID="lbtnAdd" runat="server" CssClass="btn btn-primary pull-right" OnClick="lbtnAdd_Click"><i class="fa fa-pencil-square"></i>&nbsp;Add</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:GridView ID="gvLines" AutoGenerateColumns="false" DataKeyNames="Issuing Store" class="table table-responsive no-padding table-bordered table-hover" runat="server"
                                                AllowSorting="True" AllowPaging="true" ShowFooter="true" PageSize="15">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="#No" SortExpression="">
                                                        <HeaderStyle Width="30px" />
                                                        <ItemTemplate>
                                                            <%# string.Format("{0}",Container.DataItemIndex + 1 +".") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="No_" HeaderText="Item No." />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                                    <asp:BoundField DataField="StoreName" HeaderText="Store" />
                                                    <asp:BoundField DataField="Unit of Measure" HeaderText="Unit Of Measure" />
                                                    <asp:TemplateField HeaderText="Quantity Requested">
                                                        <ItemTemplate>
                                                            <%# string.Format("{0:#,##0.00}", ((decimal)Eval("[Quantity Requested]")))%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost">
                                                        <ItemTemplate>
                                                            <%# string.Format("{0:#,##0.00}", ((decimal)Eval("[varAmount]")))%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action" SortExpression="" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemStyle Width="110px" HorizontalAlign="Left" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnRemove" CssClass="label label-danger" runat="server" ToolTip="Click to Remove line" OnClick="lbtnRemove_Click" OnClientClick="return confirm('Are you sure you want to delete this line?')" CommandArgument='<%# Eval("Line No_") %>'><i class="fa fa-remove"></i> Remove</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle HorizontalAlign="Center" />
                                                <EmptyDataTemplate>
                                                    <span style="color: red">No Recods</span>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <a href="StoreListing.aspx" class="btn btn-warning pull-left"><i class="fa fa-backward"></i>&nbsp;Back</a>
                                            <div class="pull-right">
                                                <asp:LinkButton ID="lbtnApproval" runat="server" CssClass="btn btn-success" OnClick="lbtnApproval_Click"><i class="fa fa-paper-plane"></i>&nbsp;Send Approval</asp:LinkButton>
                                                <asp:LinkButton ID="lbtnCancel" runat="server" CssClass="btn btn-danger" OnClick="lbtnCancel_Click"><i class="fa fa-trash-o"></i>Cancel Approval</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
