<%@ Page Title="Approval Tracking" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="ApprovalTracking.aspx.cs" Inherits="KSAStaff.pages.ApprovalTracking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
     <div class="content-wrapper">
        <section class="content-header">
            <h1>Approval Tracking</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Approval Tracking</li>
            </ol>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-12">
                   <div class="table-responsive">
                      <table id="example1" class="table no-margin">
                        <thead>
                            <tr>
                                <th class="small">#</th>
                                <th class="small">Entry No.</th>
                                <th class="small">Sequence No.</th>
                                <th class="small">Date-Time Sent for Approval</th>
                                <th class="small">Sender Name</th>
                                <th class="small">Approver Name</th>
                                <th class="small">Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%=ApprovalTracks()%>
                        </tbody>
                    </table>
                  </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
