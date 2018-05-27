import { AppState } from './store';

declare global {
    interface Window {
        initialReduxState: AppState;
    }
    const __REDUX_DEVTOOLS_EXTENSION_COMPOSE__: any;
}