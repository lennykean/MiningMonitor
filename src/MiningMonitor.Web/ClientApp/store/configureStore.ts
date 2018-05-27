import { routerMiddleware } from 'react-router-redux';
import { applyMiddleware, compose, createStore } from 'redux';
import immutableStateInvariantMiddleware from 'redux-immutable-state-invariant';
import thunk from 'redux-thunk';

import { History } from 'history';

import { AppState } from '.';
import { rootReducer } from '../reducers';

function configureStoreProd(history: History, initialState?: AppState) {
    const middleware = [
        routerMiddleware(history),
        thunk,
    ];
    return createStore(rootReducer, initialState, compose(applyMiddleware(...middleware)));
}

function configureStoreDev(history: History, initialState?: AppState) {
    const middleware = [
        routerMiddleware(history),
        thunk,
        immutableStateInvariantMiddleware(),
    ];

    let composeEnhancers = compose;

    if (typeof __REDUX_DEVTOOLS_EXTENSION_COMPOSE__ !== 'undefined') {
        composeEnhancers = __REDUX_DEVTOOLS_EXTENSION_COMPOSE__;
    }

    return createStore(rootReducer, initialState, composeEnhancers(applyMiddleware(...middleware)));
}

export const configureStore = process.env.NODE_ENV === 'production' ? configureStoreProd : configureStoreDev;
