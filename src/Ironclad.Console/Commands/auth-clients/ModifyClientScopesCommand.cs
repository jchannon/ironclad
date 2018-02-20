﻿// Copyright (c) Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Ironclad.Console.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using McMaster.Extensions.CommandLineUtils;

    internal class ModifyClientScopesCommand : ICommand
    {
        private string clientId;
        private List<string> scopes;

        private ModifyClientScopesCommand()
        {
        }

        public static void Configure(CommandLineApplication app, CommandLineOptions options)
        {
            // description
            app.Description = "Modifies the scopes for the specified client";
            app.HelpOption();

            // arguments
            var argumentClientId = app.Argument("id", "The client ID", false);
            var argumentScopes = app.Argument("scopes", "One or more scopes to assign to the client", true);

            // action (for this command)
            app.OnExecute(
                () =>
                {
                    if (string.IsNullOrEmpty(argumentClientId.Value) || !argumentScopes.Values.Any())
                    {
                        app.ShowHelp();
                        return;
                    }

                    options.Command = new ModifyClientScopesCommand { clientId = argumentClientId.Value, scopes = argumentScopes.Values };
                });
        }

        public async Task ExecuteAsync(CommandContext context)
        {
            var client = new Ironclad.Client.Client
            {
                Id = this.clientId,
                AllowedScopes = this.scopes,
            };

            await context.ClientsClient.ModifyClientAsync(client).ConfigureAwait(false);
        }
    }
}