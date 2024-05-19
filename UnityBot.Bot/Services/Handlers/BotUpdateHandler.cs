﻿using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UnityBot.Bot.Services.UserServices;

namespace UnityBot.Bot.Services.Handlers
{
    public partial class BotUpdateHandler : IUpdateHandler
    {
        private readonly ILogger<BotUpdateHandler> _logger;
        private readonly IUserService _userService;

        public BotUpdateHandler(ILogger<BotUpdateHandler> logger, UserServices.IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Error occured with telegramBot {exception}");

            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update.Message, cancellationToken),
                _ => HandleUnknownMessageAsync(botClient, update, cancellationToken)
            };
            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                await handler;
            }
        }

        private async Task HandleUnknownMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Stop doing this!👎",
                cancellationToken: cancellationToken);
        }
    }
}
