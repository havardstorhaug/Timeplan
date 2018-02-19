<%@ Page Title="Trinn" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrinnList.aspx.cs" Inherits="Timeplan.Lists.TrinnList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <br />

    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID="SaveButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" CausesValidation="false" Text="Avbryt" OnClick="CancelButton_Click" ID="CancelButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Opprett nytt trinn" OnClick="CreateNewButton_Click" ID="CreateNewButtonTop" />

    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>

            <asp:ListView
                ID="TrinnListView"
                runat="server"
                ItemPlaceholderID="TrinnItem"
                OnSorting="ListViewEvents_Sorting">

                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tbody>
                            <tr class="listview-heading">
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Id" ID="IdLinkButton" runat="server" Visible="False">Id</asp:LinkButton>
                                    <div class="show-hide">
                                        <%--TODO: better descrition/view of show hide func--%>
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowIdLinkButton" Visible="False"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th class="form-group">
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Navn" ID="NavnLinkButton" runat="server">Navn</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowNavnLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="UkeTimeTall" ID="UkeTimeTallLinkButton" runat="server">Uketimetall</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowUkeTimeTallLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Elever" ID="EleverLinkButton" runat="server">Elever</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowEleverLinkButton"></asp:LinkButton>&nbsp;
                                    </div>
                                </th>
                                
                                <th></th>
                                <th></th>
                                <th></th>
                            </tr>
                            <tr id="TrinnItem" runat="server"></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>

                <EmptyItemTemplate>
                    <p>
                        Ingen trinn funnet
                    </p>
                </EmptyItemTemplate>

                <%--TODO: whats this?--%>
                <%--<GroupTemplate>
                    <tr id="itemPlaceholderContainer" runat="server">
                        <td id="itemPlaceholder" runat="server"></td>
                    </tr>
                </GroupTemplate>--%>

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
                                <asp:TextBox CssClass="form-control" runat="server" ID="UkeTimeTallTextBox" MaxLength="5" Text='<%# Eval("UkeTimeTall") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RangeValidator runat="server" ID="UkeTimeTallRangeValidator" ControlToValidate="UkeTimeTallTextBox" Type="Double" MinimumValue="0" MaximumValue="100" Display="Dynamic" ErrorMessage="Ugyldig antall" />
                                    <asp:RequiredFieldValidator runat="server" ID="UkeTimeTallRequiredFieldValidator" ControlToValidate="UkeTimeTallTextBox" Display="Dynamic" ErrorMessage="Påkrevd" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:ListBox CssClass="form-control" runat="server" SelectionMode="Multiple" Visible="True" ID="EleverListBox" Height="34"></asp:ListBox>
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
                                    CommandArgument='<%# Eval("Id") %>' OnClick="DeleteButton_Click" ID="DeleteLinkButton" ToolTip="Slett trinn">
                                    <span class="glyphicon glyphicon-remove"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-edit">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="EditButton_Click" ID="EditLinkButton" ToolTip="Rediger trinn i detaljvisning">
                                    <span class="glyphicon glyphicon-pencil"></span>
                                </asp:LinkButton>
                                <%--<div class="correct-padding"></div>--%>
                            </div>
                        </td>

                        <td></td>
                    </tr>
                </ItemTemplate>

            </asp:ListView>

        </div>

    </section>

    <br />

    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID="SaveButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Avbryt" CausesValidation="false" OnClick="CancelButton_Click" ID="CancelButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Opprett nytt trinn" OnClick="CreateNewButton_Click" ID="CreateNewButtonBottom" />

    <script src="../Scripts/plugins/checkboxDropdown/bootstrap-multiselect.js"></script>
    <script>

        $(function () {
            $("[id*=EleverListBox]").multiselect({
                numberDisplayed: 3,
                maxHeight: 350,
                disableRemove: true
            });
        });

        function ConfirmDelete(name) {
            if (confirm("Er du sikker på at du vil slette trinn '" + name + "'?")) {
                return true;
            }
            return false;
        }

    </script>

</asp:Content>
