<%@ Page Title="Sfo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SfoList.aspx.cs" Inherits="Timeplan.Lists.SfoList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      
    <br />

    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID="SaveButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Avbryt" CausesValidation="false" OnClick="CancelButton_Click" ID="CancelButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Opprett ny sfo" OnClick="CreateNewButton_Click" ID="CreateNewButtonTop" />

    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>

            <asp:ListView
                ID="SfoListView"
                runat="server"
                ItemPlaceholderID="SfoItem"
                OnSorting="ListViewEvents_Sorting">

                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tbody>
                            <tr class="listview-heading">
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Id" ID="IdLinkButton" runat="server" Visible="False">Id</asp:LinkButton>
                                    <div class="show-hide">
                                        <%--TODO: better descrition/view of show hide func--%>
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowIdLinkButton" Visible="False"></asp:LinkButton>
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Navn" ID="NavnLinkButton" runat="server">Navn</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowNavnLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Elever" ID="EleverLinkButton" runat="server">Elever</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowEleverLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Ansatte" ID="AnsatteLinkButton" runat="server">Ansatte</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowAnsatteLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                
                                <th></th>
                                <th></th>
                                <th></th>
                            </tr>
                            <tr id="SfoItem" runat="server"></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>

                <EmptyItemTemplate>
                    <p>
                        Ingen elever funnet
                    </p>
                </EmptyItemTemplate>


                <ItemTemplate>
                    <tr class="row-standard">
                        <td>
                            <div class="form-group">
                                <asp:Button CssClass="form-control" runat="server" ID="IdButton" Text='<%# Eval("Id") %>' Visible="False"></asp:Button>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" runat="server" ID="NavnTextBox" MaxLength="100" Text='<%# Eval("Navn") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RequiredFieldValidator runat="server" ID="NavnRequiredFieldValidator" Display="Dynamic" ControlToValidate="NavnTextBox" ErrorMessage="Påkrevd" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:ListBox CssClass="form-control" runat="server" SelectionMode="Multiple" Visible="True" ID="EleverListBox" Height="1"></asp:ListBox>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:ListBox CssClass="form-control" runat="server" SelectionMode="Multiple" Visible="True" ID="AnsatteListBox" Height="1"></asp:ListBox>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-save">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="SaveButton_Click" ID="SaveLinkButton" ToolTip="Lagre endringer">
                                    <span class="glyphicon glyphicon-floppy-saved"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-delete">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" 
                                    OnClientClick='<%# String.Format("javascript:return ConfirmDelete(\"{0}\")", Eval("Navn") != null ? Eval("Navn").ToString() : string.Empty) %>' 
                                    CommandArgument='<%# Eval("Id") %>' OnClick="DeleteButton_Click" ID="DeleteLinkButton" ToolTip="Slett sfo">
                                    <span class="glyphicon glyphicon-remove"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                        
                        </td>
                    </tr>
                </ItemTemplate>

            </asp:ListView>
        
        </div>

    </section>

    <br />

    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID="SaveButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Avbryt" CausesValidation="false" OnClick="CancelButton_Click" ID="CancelButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Opprett ny sfo" OnClick="CreateNewButton_Click" ID="CreateNewButtonBottom" />

    <script type="text/javascript" src="../Scripts/plugins/checkboxDropdown/bootstrap-multiselect.js"></script>
    <script type="text/javascript">

        $(function () {
            $("[id*=EleverListBox]").multiselect({
                numberDisplayed: 3,
                maxHeight: 350,
            });
        });

        $(function () {
            $("[id*=AnsatteListBox]").multiselect({
                numberDisplayed: 3,
                maxHeight: 350,
            });
        });

        function ConfirmDelete(name) {
            if (confirm("Er du sikker på at du vil slette sfo '" + name + "'?")) {
                return true;
            }
            return false;
        }

        function ScrollToBottom() {
            window.scrollTo(0, document.body.scrollHeight);
        }

    </script>

</asp:Content>
