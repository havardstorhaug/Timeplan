<%@ Page Title="Rediger elev" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ElevDetails.aspx.cs" Inherits="Timeplan.Details.ElevDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<div>
        <h1>Rediger elev</h1>
    </div>--%>

    <br />

    <%--<asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" OnClick="AddElevTilstedeButton_Click" ID="AddElevTilstedeButton" Text="Opprett ny tilstedeværelse" />--%>

    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>

            <asp:ListView
                ID="ElevTilstedeListView"
                runat="server"
                ItemPlaceholderID="ElevTilstedeItem">

                <%--OnSorting="ListViewEvents_Sorting"--%>

                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tbody>
                            <tr class="listview-heading">
                                <th>
                                    <asp:Label ID="IdLabel" runat="server" Visible="False">Id</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="MandagStartLabel" runat="server">Mandag start</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="MandagSluttLabel" runat="server">Mandag slutt</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TirsdagStartLabel" runat="server">Tirsdag start</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TirsdagSluttLabel" runat="server">Tirsdag slutt</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="OnsdagStartLabel" runat="server">Onsdag start</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="OnsdagSluttLabel" runat="server">Onsdag slutt</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TorsdagStartLabel" runat="server">Torsdag start</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TorsdagSluttLabel" runat="server">Torsdag slutt</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="FredagStartLabel" runat="server">Fredag start</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="FredagSluttLabel" runat="server">Fredag slutt</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="UkeTypeLabel" runat="server">Uketype &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                                </th>

                                <th></th>
                                <th></th>
                                <th></th>
                            </tr>
                            <tr id="ElevTilstedeItem" runat="server"></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>

                <EmptyItemTemplate>
                    <p>
                        Ingen elever funnet
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
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="MandagStartTextBox" Text='<%# Eval("MandagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="MandagStartRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="MandagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="MandagSluttTextBox" Text='<%# Eval("MandagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="MandagSluttRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="MandagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="TirsdagStartTextBox" Text='<%# Eval("TirsdagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="Dynamic" ControlToValidate="TirsdagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="TirsdagSluttTextBox" Text='<%# Eval("TirsdagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic" ControlToValidate="TirsdagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="OnsdagStartTextBox" Text='<%# Eval("OnsdagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic" ControlToValidate="OnsdagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="OnsdagSluttTextBox" Text='<%# Eval("OnsdagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="Dynamic" ControlToValidate="OnsdagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="TorsdagStartTextBox" Text='<%# Eval("TorsdagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" Display="Dynamic" ControlToValidate="TorsdagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="TorsdagSluttTextBox" Text='<%# Eval("TorsdagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" Display="Dynamic" ControlToValidate="TorsdagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="FredagStartTextBox" Text='<%# Eval("FredagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" Display="Dynamic" ControlToValidate="FredagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" MaxLength="5" ID="FredagSluttTextBox" Text='<%# Eval("FredagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" Display="Dynamic" ControlToValidate="FredagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="UkeTypeDropDown"></asp:DropDownList>
                                <div class="correct-padding"></div>
                            </div>
                        </td>

                        <td>
                            <div class="form-group btn-delete">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="DeleteElevTilstedeButton_Click" ID="DeleteElevTilstedeLinkButton" ToolTip="Slett tilstedeværelse">
                                    <span class="glyphicon glyphicon-remove"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-save">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="AddElevTilstedeButton_Click" ID="AddElevTilstedeButton" ToolTip="Opprett ny tilstedeværelse">
                                    <span class="glyphicon glyphicon-plus"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>

                        <td></td>
                    </tr>
                </ItemTemplate>

            </asp:ListView>

        </div>

    </section>

    <br />

    <section>
        <div class="row">
            <div class="form-group">
                <%--<label>Id</label>--%>
                <asp:TextBox CssClass="form-control" runat="server" ReadOnly="True" Visible="false" ID="IdTextBox" />
            </div>
            <div class="form-group col-md-3">
                <label>Navn</label>
                <asp:TextBox CssClass="form-control" runat="server" ID="NavnTextBox" MaxLength="100" />
                <div class="error-message">
                    <asp:RequiredFieldValidator runat="server" ID="NavnRequiredFieldValidator" Display="Dynamic" ControlToValidate="NavnTextBox" ErrorMessage="Påkrevd" />
                </div>
            </div>
            <div class="form-group col-md-3">
                <label>Klasse</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" ID="KlasseDropDown"></asp:DropDownList>
            </div>
            <div class="form-group col-md-3">
                <label>Klassetrinn</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" AutoPostBack="true" ID="TrinnDropDown" OnSelectedIndexChanged="TrinnDropDown_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group col-md-3">
                <label>Skole t/uke</label>
                <asp:TextBox CssClass="form-control" runat="server" ID="SkoleTimerPrUkeTextBox" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-md-3">
                <label>Hovedpedagog</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" ID="HovedPedagogDropDown"></asp:DropDownList>
            </div>
            <div class="form-group col-md-3">
                <label>Norm skole</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" ID="BemanningsNormSkoleDropDown"></asp:DropDownList>
            </div>
            <div class="form-group col-md-3">
                <label>Sfo avdeling</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" ID="SfoDropDown" AutoPostBack="true" OnSelectedIndexChanged="SfoDropDown_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group col-md-3">
                <label>Sfo %</label>
                <asp:TextBox CssClass="form-control form-control-max-width" runat="server" ID="SfoProsentTextBox" Text='<%# Eval("SfoProsent") %>'></asp:TextBox>
                <div class="error-message">
                    <asp:RangeValidator runat="server" ID="SfoProsentRangeValidator" ControlToValidate="SfoProsentTextBox" Type="Double" MinimumValue="0" MaximumValue="100" Display="Dynamic" ErrorMessage="Ugyldig %" />
                    <asp:RequiredFieldValidator runat="server" ID="SfoProsentRequiredFieldValidator" ControlToValidate="SfoProsentTextBox" Display="Dynamic" ErrorMessage="Påkrevd" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-md-3">
                <label>Norm sfo</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" ID="BemanningsNormSfoDropDown"></asp:DropDownList>
            </div>
            <div class="form-group col-md-3">
                <label>Tlfnr</label>
                <asp:TextBox CssClass="form-control" runat="server" ID="TlfnrTextBox" MaxLength="100" />
            </div>
           
        </div>

    </section>

    <br />

    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Lagre" OnClick="SaveButton_Click" ID="SaveButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Avbryt" OnClick="CancelButton_Click" ID="CancelButtonBottom" CausesValidation="false" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Opprett ny elev" OnClick="CreateNewButton_Click" ID="CreateNewButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" OnClientClick="return ConfirmDelete()" Text="Slett elev" OnClick="DeleteButton_Click" ID="DeleteButton" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Tilbake" OnClick="GoBackButton_Click" ID="GoBackButton" />

    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Neste" OnClick="NextButton_Click" ID="NextButton" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Forrige" OnClick="PreviousButton_Click" ID="PreviousButton" />
    
    <script type="text/javascript">

        function ConfirmDelete() {
            if (confirm("Er du sikker på at du vil slette elev?")) {
                return true;
            }
            return false;
        }

    </script>

</asp:Content>
