import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { ConnectedRouter } from 'react-router-redux';

import { createBrowserHistory } from 'history';

import { Main } from './components';
import { configureStore } from './store';

import '../sass/mining-monitor.scss';

const history = createBrowserHistory();
const store = configureStore(history, window.initialReduxState);

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <Main />
        </ConnectedRouter>
    </Provider>,
    document.getElementById('miningmonitor'),
);
