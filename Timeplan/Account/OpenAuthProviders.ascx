<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpenAuthProviders.ascx.cs" Inherits="Timeplan.Account.OpenAuthProviders" %>

<div id="socialLoginList">
    <h4>Benytt en annen tjeneste for innlogging.</h4>
    <hr />
    <asp:ListView runat="server" ID="providerDetails" ItemType="System.String"
        SelectMethod="GetProviderNames" ViewStateMode="Disabled">
        <ItemTemplate>
            <p>
                <button type="submit" class="btn btn-default" name="provider" value="<%#: Item %>"
                    title="Logg inn med din <%#: Item %> konto.">
                    <%#: Item %>
                </button>
            </p>
        </ItemTemplate>
        <EmptyDataTemplate>
            <div>
                <p>Det er ikke satt opp mulighet for pålogging via eksterne tjenester. Se <a href="http://go.microsoft.com/fwlink/?LinkId=252803">denne artikkel</a> for detaljer rundt oppsett av innlogging via eksterne tjenster.</p>
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
</div>
