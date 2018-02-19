<%@ Page Title="Rediger ansatt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnsattDetails.aspx.cs" Inherits="Timeplan.Details.AnsattDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<div>
        <h1>Rediger ansatt</h1e
    </div>--%>

    <br />

    <%--<asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" OnClick="AddAnsattTilstedeButton_Click" ID="AddAnsattTilstedeButton" Text="Opprett ny tilstedeværelse" />--%>

    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>

            <asp:ListView
                ID="AnsattTilstedeListView"
                runat="server"
                ItemPlaceholderID="AnsattTilstedeItem">

                <%--OnSorting="ListViewEvents_Sorting"--%>

                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tbody>
                            <%--<tr class="listview-heading">
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
                                    <asp:Label ID="MandagFriLabel" runat="server">Fri</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TirsdagStartLabel" runat="server">Tirsdag start</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TirsdagSluttLabel" runat="server">Tirsdag slutt</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TirsdagFriLabel" runat="server">Fri</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="OnsdagStartLabel" runat="server">Onsdag start</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="OnsdagSluttLabel" runat="server">Onsdag slutt</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="OnsdagFriLabel" runat="server">Fri</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TorsdagStartLabel" runat="server">Torsdag start</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TorsdagSluttLabel" runat="server">Torsdag slutt</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="TorsdagFriLabel" runat="server">Fri</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="FredagStartLabel" runat="server">Fredag start</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="FredagSluttLabel" runat="server">Fredag slutt</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="FredagFriLabel" runat="server">Fri</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="SkoleLabel" runat="server">Skole</asp:Label>
                                </th>
                                <th>
                                    <asp:Label ID="UkeTypeLabel" runat="server">Uketype &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                                </th>

                                <th></th>
                                <th></th>
                                <th></th>
                            </tr>--%>
                            <tr id="AnsattTilstedeItem" runat="server"></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>

                <EmptyItemTemplate>
                    <p>
                        Ingen ansatte funnet
                    </p>
                </EmptyItemTemplate>

                <%--TODO: whats this?--%>
                <%--<GroupTemplate>
                    <tr id="itemPlaceholderContainer" runat="server">
                        <td id="itemPlaceholder" runat="server"></td>
                    </tr>
                </GroupTemplate>--%>

                <ItemTemplate>
                    <div class="border">

                        <div class="row">
                            <div class="form-group">
                                <asp:Button CssClass="form-control" runat="server" ID="IdButton" Text='<%# Eval("Id") %>' Visible="False"></asp:Button>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Mandag start</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="MandagStartTextBox" MaxLength="5" Text='<%# Eval("MandagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="MandagStartRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="MandagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Mandag slutt</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="MandagSluttTextBox" MaxLength="5" Text='<%# Eval("MandagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="MandagSluttRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="MandagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Fri</label>
                                <asp:CheckBox CssClass="form-control form-control-checkbox-max-width" runat="server" ID="MandagFriCheckBox" Checked='<%# Eval("MandagFri") %>'></asp:CheckBox>
                                <div class="correct-padding"></div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Tirsdag start</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="TirsdagStartTextBox" MaxLength="5" Text='<%# Eval("TirsdagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="TirsdagStartRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="TirsdagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Tirsdag slutt</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="TirsdagSluttTextBox" MaxLength="5" Text='<%# Eval("TirsdagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="TirsdagSluttRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="TirsdagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Fri</label>
                                <asp:CheckBox CssClass="form-control" runat="server" ID="TirsdagFriCheckBox" Checked='<%# Eval("TirsdagFri") %>'></asp:CheckBox>
                                <div class="correct-padding"></div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="form-group col-md-2">
                                <label>Onsdag start</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="OnsdagStartTextBox" MaxLength="5" Text='<%# Eval("OnsdagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="OnsdagStartRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="OnsdagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Onsdag slutt</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="OnsdagSluttTextBox" MaxLength="5" Text='<%# Eval("OnsdagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="OnsdagSluttRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="OnsdagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Fri</label>
                                <asp:CheckBox CssClass="form-control" runat="server" ID="OnsdagFriCheckBox" Checked='<%# Eval("OnsdagFri") %>'></asp:CheckBox>
                                <div class="correct-padding"></div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Torsdag start</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="TorsdagStartTextBox" MaxLength="5" Text='<%# Eval("TorsdagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="TorsdagStartRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="TorsdagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Torsdag slutt</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="TorsdagSluttTextBox" MaxLength="5" Text='<%# Eval("TorsdagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="TorsdagSluttRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="TorsdagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Fri</label>
                                <asp:CheckBox CssClass="form-control" runat="server" ID="TorsdagFriCheckBox" Checked='<%# Eval("TorsdagFri") %>'></asp:CheckBox>
                                <div class="correct-padding"></div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="form-group col-md-2">
                                <label>Fredag start</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="FredagStartTextBox" MaxLength="5" Text='<%# Eval("FredagStart") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="FredagStartRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="FredagStartTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Fredag slutt</label>
                                <asp:TextBox CssClass="form-control" TextMode="time" runat="server" ID="FredagSluttTextBox" MaxLength="5" Text='<%# Eval("FredagSlutt") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RegularExpressionValidator ID="FredagSluttRegularExpressionValidator" runat="server" Display="Dynamic" ControlToValidate="FredagSluttTextBox" ErrorMessage="Ugyldig tid"
                                        ValidationExpression="^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Fri</label>
                                <asp:CheckBox CssClass="form-control" runat="server" ID="FredagFriCheckBox" Checked='<%# Eval("FredagFri") %>'></asp:CheckBox>
                                <div class="correct-padding"></div>
                            </div>

                            <div class="form-group col-md-2">
                                <label>Skole</label>
                                <asp:CheckBox CssClass="form-control" runat="server" ID="SkoleCheckBox" Checked='<%# Eval("Skole") %>'></asp:CheckBox>
                                <div class="correct-padding"></div>
                            </div>
                            <div class="form-group col-md-2">
                                <label>Uketype</label>
                                <asp:DropDownList CssClass="form-control" runat="server" ID="UkeTypeDropDown"></asp:DropDownList>
                                <div class="correct-padding"></div>
                            </div>
                            <div class="form-group col-md-1 btn-delete">
                                <div class="correct-padding-temp"></div>
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="DeleteAnsattTilstedeButton_Click" ID="DeleteAnsattTilstedeLinkButton" ToolTip="Slett tilstedeværelse">
                                    <span class="glyphicon glyphicon-remove"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                            <div class="form-group col-md-1 btn-save">
                                <div class="correct-padding-temp"></div>
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="AddAnsattTilstedeButton_Click" ID="AddAnsattTilstedeButton" ToolTip="Opprett ny tilstedeværelse">
                                    <span class="glyphicon glyphicon-plus"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </div>

                    </div>
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
                <asp:TextBox CssClass="form-control" runat="server" ID="NavnTextBox" />
                <div class="error-message">
                    <asp:RequiredFieldValidator runat="server" ID="NavnRequiredFieldValidator" Display="Dynamic" ControlToValidate="NavnTextBox" ErrorMessage="Påkrevd" />
                </div>
            </div>
            <div class="form-group col-md-3">
                <label>Stilling (%)</label>
                <asp:TextBox CssClass="form-control" runat="server" ID="StillingsStørrelseTextBox"></asp:TextBox>
                <div class="error-message">
                    <asp:RangeValidator runat="server" ID="StillingsStørrelseRangeValidator" ControlToValidate="StillingsStørrelseTextBox" Type="Double" MinimumValue="0" MaximumValue="100" Display="Dynamic" ErrorMessage="Ugyldig %" />
                    <asp:RequiredFieldValidator runat="server" ID="StillingsStørrelseRequiredFieldValidator" ControlToValidate="StillingsStørrelseTextBox" Display="Dynamic" ErrorMessage="Påkrevd" />
                </div>
            </div>
            <div class="form-group col-md-3">
                <label>Timer pr. uke</label>
                <asp:TextBox CssClass="form-control" runat="server" ID="TimerPrUkeTextBox" ReadOnly="true"></asp:TextBox>
            </div>
            <div class="form-group col-md-3">
                <label>Diff timer</label>
                <asp:TextBox CssClass="form-control" runat="server" ID="DiffTimerTextBox" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="form-group col-md-3">
                <label>Tlfnr</label>
                <asp:TextBox CssClass="form-control form-control-max-width" runat="server" ID="TlfNrTextBox"></asp:TextBox>
                <div class="error-message">
                    <asp:RangeValidator runat="server" ID="TlfnrRangeValidator" ControlToValidate="TlfNrTextBox" Type="Double" MinimumValue="10000000" MaximumValue="99999999" Display="Dynamic" ErrorMessage="Ugyldig nr" />
                </div>
            </div>
            <div class="form-group col-md-3">
                <label>Avdeling</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" ID="AvdelingDropDown"></asp:DropDownList>
            </div>
            <div class="form-group col-md-3">
                <label>Stillingstype</label>
                <asp:DropDownList CssClass="form-control form-control-max-width" runat="server" ID="StillingsTypeDropDown"></asp:DropDownList>
            </div>
            <div class="form-group col-md-3">
                <label>Klasser</label><br />
                <asp:ListBox CssClass="jquery-search form-control" runat="server" SelectionMode="Multiple" Visible="True" ID="JobberIKlasserListBox" Height="75"></asp:ListBox>
            </div>

        </div>
        <div class="row">
            <div class="form-group col-md-3">
                <label>Sfo</label><br />
                <asp:ListBox CssClass="jquery-search form-control" runat="server" SelectionMode="Multiple" Visible="True" ID="JobberISfosListBox" Height="75"></asp:ListBox>
            </div>
        </div>

    </section>

    <br />

    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Lagre" OnClick="SaveButton_Click" ID="SaveButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" CausesValidation="false" Text="Avbryt" OnClick="CancelButton_Click" ID="CancelButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Opprett ny ansatt" OnClick="CreateNewButton_Click" ID="CreateNewButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" OnClientClick="return ConfirmDelete()" Text="Slett ansatt" OnClick="DeleteButton_Click" ID="DeleteButton" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Tilbake" OnClick="GoBackButton_Click" ID="GoBackButton" />

    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Neste" OnClick="NextButton_Click" ID="NextButton" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Forrige" OnClick="PreviousButton_Click" ID="PreviousButton" />

    <script type="text/javascript">

        $(function () {
            $("select.jquery-search").multiselect({
                numberDisplayed: 3,
                maxHeight: 350,
            });
        });

        function ConfirmDelete() {
            if (confirm("Er du sikker på at du vil slette ansatt?")) {
                return true;
            }
            return false;
        }

    </script>

    <script type="text/javascript" src="../Scripts/plugins/checkboxDropdown/bootstrap-multiselect.js"></script>

</asp:Content>
