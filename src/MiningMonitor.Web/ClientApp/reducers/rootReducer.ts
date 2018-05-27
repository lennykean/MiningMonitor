import { routerReducer } from 'react-router-redux';
import { combineReducers } from 'redux';

import {
    busyReducer,
    collectorReducer,
    loginReducer,
    minerReducer,
    settingsReducer,
    snapshotReducer,
    userReducer,
    validationReducer,
} from '.';

export const rootReducer = combineReducers({
    busy: busyReducer,
    collector: collectorReducer,
    login: loginReducer,
    miner: minerReducer,
    router: routerReducer,
    settings: settingsReducer,
    snapshot: snapshotReducer,
    user: userReducer,
    validation: validationReducer,
});
