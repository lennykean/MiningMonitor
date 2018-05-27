import * as React from 'react';
import { renderToString } from 'react-dom/server';
import { Provider } from 'react-redux';
import { StaticRouter } from 'react-router';
import { replace } from 'react-router-redux';

import { createMemoryHistory } from 'history';

import { createServerRenderer, RenderResult } from 'aspnet-prerendering';

import { Main } from './components';
import { configureStore } from './store';

export default createServerRenderer(async (params) => {
    const basename = params.baseUrl.substring(0, params.baseUrl.length - 1);
    const urlAfterBasename = params.url.substring(basename.length);
    const store = configureStore(createMemoryHistory());

    store.dispatch(replace(urlAfterBasename));

    const app = (
        <Provider store={store}>
            <StaticRouter basename={basename} context={{}} location={params.location.path}>
                <Main />
            </StaticRouter>
        </Provider>);

    renderToString(app);

    await params.domainTasks;

    return {
        html: renderToString(app),
        globals: {
            initialReduxState: store.getState(),
        },
    };
});
