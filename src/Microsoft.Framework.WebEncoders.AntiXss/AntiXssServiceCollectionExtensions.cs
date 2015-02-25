// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.Internal;
using Microsoft.Framework.WebEncoders;

namespace Microsoft.Framework.DependencyInjection
{
    public static class AntiXssServiceCollectionExtensions
    {
        private static AntiXssEncoder _encoder;

        /// <summary>
        /// Configures the Microsoft Web Protection Library's AntiXSS encoders as the default encoders.
        /// </summary>
        public static IServiceCollection AddAntiXssEncoders([NotNull] this IServiceCollection services)
        {
            return AddAntiXssEncoders(services, configuration: null);
        }

        /// <summary>
        /// Configures the Microsoft Web Protection Library's AntiXSS encoders as the default encoders.
        /// </summary>
        public static IServiceCollection AddAntiXssEncoders([NotNull] this IServiceCollection services, IConfiguration configuration)
        {
            // First, make sure the default encoders are registered.
            // This calls TryAdd on the instances so that existing descriptors aren't overwritten.
            services.AddEncoders(configuration);

            // Finally, register our concrete implementation that calls into AntiXSS.
            // We want to overwrite any existing descriptor instances since presumably the developer
            // wouldn't have called this API unless he meant to perform a full replacement.
            //
            // Finally, even though AntiXSS has a JavaScriptEncode method, it's not suitable for
            // JSON strings, so we can't use it.
            Func<IServiceProvider, AntiXssEncoder> encoderFactory = _ => LazyInitializer.EnsureInitialized(ref _encoder);
            services.AddSingleton<IHtmlEncoder>(encoderFactory);
            services.AddSingleton<IUrlEncoder>(encoderFactory);

            return services;
        }
    }
}
