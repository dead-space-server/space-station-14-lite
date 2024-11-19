using Robust.Client;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;

namespace Content.Client.Info
{
    public sealed class ServerListBox : BoxContainer
    {
        private IGameController _gameController;
        private List<Button> _connectButtons = new();

        public ServerListBox()
        {
            _gameController = IoCManager.Resolve<IGameController>();
            Orientation = LayoutOrientation.Vertical;
            AddServers();
        }

        private void AddServers()
        {
            AddServerInfo("Титан", "Проект с упором на РП");
            AddServerInfo("Деймос", "Проект с частыми ивентами и сбалансированным игровым опытом");
            AddServerInfo("Союз-1", "Проект в сеттинге СССП");
            AddServerInfo("Фронтир", "Проект на тематику космических путешествий и торговли");
        }

        private void AddServerInfo(string serverName, string description)
        {
            var serverBox = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
            };

            var nameAndDescriptionBox = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
            };

            var serverNameLabel = new Label
            {
                Text = serverName,
                MinWidth = 200
            };

            var descriptionLabel = new RichTextLabel
            {
                MaxWidth = 350
            };
            descriptionLabel.SetMessage(FormattedMessage.FromMarkup(description));

            var buttonBox = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
                HorizontalExpand = true,
                HorizontalAlignment = HAlignment.Right
            };

            var connectButton = new Button
            {
                Text = "Подключиться"
            };

            _connectButtons.Add(connectButton);

            connectButton.OnPressed += _ =>
            {
                ConnectToServer(serverName);

                foreach (var connectButton in _connectButtons)
                {
                    connectButton.Disabled = true;
                }
            };

            buttonBox.AddChild(connectButton);

            nameAndDescriptionBox.AddChild(serverNameLabel);
            nameAndDescriptionBox.AddChild(descriptionLabel);

            serverBox.AddChild(nameAndDescriptionBox);
            serverBox.AddChild(buttonBox);

            AddChild(serverBox);
        }

        private void ConnectToServer(string serverName)
        {
            var url = "";

            switch (serverName)
            {
                case "Деймос":
                    url = "ss14://f3.deadspace14.net:1213";
                    break;
                case "Титан":
                    url = "ss14://f2.deadspace14.net:1212";
                    break;
                case "Союз-1":
                    url = "ss14://s1.deadspace14.net:1213";
                    break;
                case "Фронтир":
                    url = "ss14://ff.deadspace14.net:1213";
                    break;
            }
            _gameController.Redial(url, "Connecting to another server...");
        }
    }
}
