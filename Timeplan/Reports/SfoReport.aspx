<%@ Page Title="Sfo rapport" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SfoReport.aspx.cs" Inherits="Timeplan.Reports.SfoReport" %>

<%--<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />

    <section>
        <div class="row">
            <div class="form-group col-md-2">
                <label>Avdelinger</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" ID="SfoAvdelingDropDown" AutoPostBack="true" OnSelectedIndexChanged="SfoAvdelingDropDown_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group col-md-2">
                <label>Uketyper</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" ID="UkeTypeDropDown" AutoPostBack="true" OnSelectedIndexChanged="UkeTypeDropDown_SelectedIndexChanged"></asp:DropDownList>
            </div>
        </div>
    </section>

    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>

            <asp:ListView
                ID="SfoReportListView"
                runat="server"
                ItemPlaceholderID="SfoReportItem">

                <%--border: 1px solid black;--%>

                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tbody>
                            <tr class="listview-heading">
                                <th style="width: 5%;">
                                    <asp:Label CommandName="Sort" CommandArgument="StillingsType" ID="StillingsTypeLinkButton" runat="server">Klasse</asp:Label>
                                    <%--<div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowStillingsTypeLinkButton"></asp:LinkButton>
                                    </div>--%>
                                </th>
                                <th style="width: 19%;">
                                    <asp:Label CommandName="Sort" CommandArgument="Id" ID="IdLinkButton" runat="server">Mandag</asp:Label>
                                    <%--<div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowIdLinkButton" Visible="False"></asp:LinkButton>
                                    </div>--%>
                                </th>
                                <th style="width: 19%;">
                                    <asp:Label CommandName="Sort" CommandArgument="Navn" ID="NavnLinkButton" runat="server">Tirsdag</asp:Label>
                                    <%--<div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowNavnLinkButton"></asp:LinkButton>
                                    </div>--%>
                                </th>
                                <th style="width: 19%;">
                                    <asp:Label CommandName="Sort" CommandArgument="Stillingsstørrelse" ID="StillingsStørrelseLinkButton" runat="server">Onsdag</asp:Label>
                                    <%--<div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowStillingsStørrelseLinkButton"></asp:LinkButton>
                                    </div>--%>
                                </th>
                                <th style="width: 19%;">
                                    <asp:Label CommandName="Sort" CommandArgument="Tlfnr" ID="TlfnrLinkButton" runat="server">Torsdag</asp:Label>
                                    <%--<div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowTlfnrLinkButton"></asp:LinkButton>
                                    </div>--%>
                                </th>
                                <th style="width: 19%;">
                                    <asp:Label CommandName="Sort" CommandArgument="Avdeling" ID="AvdelingLinkButton" runat="server">Fredag</asp:Label>
                                    <%--<div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowAvdelingLinkButton"></asp:LinkButton>
                                    </div>--%>
                                </th>

                            </tr>
                            <tr id="SfoReportItem" runat="server"></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>

                <EmptyItemTemplate>
                    <p>
                        Ingen funnet
                    </p>
                </EmptyItemTemplate>

                <%--TODO: whats this?--%>
                <%--<GroupTemplate>
                    <tr id="itemPlaceholderContainer" runat="server">
                        <td id="itemPlaceholder" runat="server"></td> 
                    </tr>
                </GroupTemplate>--%>

                <ItemTemplate>
                    <tr class='<%# Eval("CSSClass") %>'>
                        <td>
                            <div class="form-group">
                                <asp:Label Font-Bold="true" runat="server" ID="KlasseLabel" Text='<%# Eval("Description") %>'></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:Label Font-Bold="true" runat="server" ID="MandagTidLabel" Text='<%# Eval("MandagTid") %>'></asp:Label>
                                <asp:Label runat="server" ID="MandagEleverLabel" Text='<%# Eval("MandagElever") %>'></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:Label Font-Bold="true" runat="server" ID="TirsdagTidLabel" Text='<%# Eval("TirsdagTid") %>'></asp:Label>
                                <asp:Label runat="server" ID="TirsdagEleverLabel" Text='<%# Eval("TirsdagElever") %>'></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:Label Font-Bold="true" runat="server" ID="OnsdagTidLabel" Text='<%# Eval("OnsdagTid") %>'></asp:Label>
                                <asp:Label runat="server" ID="OnsdagEleverLabel" Text='<%# Eval("OnsdagElever") %>'></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:Label Font-Bold="true" runat="server" ID="TorsdagTidLabel" Text='<%# Eval("TorsdagTid") %>'></asp:Label>
                                <asp:Label runat="server" ID="TorsdagEleverLabel" Text='<%# Eval("TorsdagElever") %>'></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:Label Font-Bold="true" runat="server" ID="FredagTidLabel" Text='<%# Eval("FredagTid") %>'></asp:Label>
                                <asp:Label runat="server" ID="FredagEleverLabel" Text='<%# Eval("FredagElever") %>'></asp:Label>
                            </div>
                        </td>

                    </tr>
                </ItemTemplate>

            </asp:ListView>

            <%--INFO: no need for paging on this page--%>
            <%--<div class="form-group">
                    <asp:DataPager ID="SfoReportItemDataPager" runat="server" PageSize="10" PagedControlID="AnsattListView">
                        <Fields>
                            <asp:NumericPagerField ButtonCount="2" />
                        </Fields>
                    </asp:DataPager>
                </div>--%>
        </div>

    </section>

    <%--<script type="text/javascript">

        function CalculateClass(input) {
            if (input != string.Empty) {
                return 'row-standard hr';
            }
            else if (input == "") {
                return 'row-standard'
            }
            return false;
        }

    </script>--%>

</asp:Content>
