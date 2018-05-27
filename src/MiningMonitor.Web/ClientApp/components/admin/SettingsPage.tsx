import * as React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators, Dispatch } from 'redux';

import autobind from 'autobind-decorator';

import * as _ from 'lodash';

import { SettingsForm } from '.';
import { FetchAllSettingsAction, settingsActions, UpdateSettingsAction } from '../../actions';
import { AppState, Busy, BusyAction } from '../../store';

interface Props {
    busy: Busy[];
    settings: { [key: string]: string };
    fetchAllSettings: FetchAllSettingsAction;
    updateSettings: UpdateSettingsAction;
}

export class SettingsPage extends React.Component<Props> {
    public get isLoading(): boolean {
        return _.some(this.props.busy, (busy) => busy.action === BusyAction.FetchingSettings);
    }
    public componentWillMount() {
        if (this.isLoading || this.props.settings) {
            return;
        }
        this.props.fetchAllSettings();
    }
    @autobind
    public submit(settings: { [key: string]: string }) {
        this.props.updateSettings(settings);
    }
    public render() {
        return (
            <>
                <h2>Settings</h2>
                {this.props.settings ?
                    <SettingsForm settings={this.props.settings} onSubmit={this.submit} /> :
                    null
                }
            </>
        );
    }
}

function mapStateToProps(state: AppState, ownProps: Props) {
    return {
        ...ownProps,
        busy: state.busy.busy,
        settings: state.settings.settings,
    };
}

function mapDispatchToProps(dispatch: Dispatch<AppState>) {
    return {
        fetchAllSettings: bindActionCreators(settingsActions.fetchAllSettings, dispatch),
        updateSettings: bindActionCreators(settingsActions.updateSettings, dispatch),
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(SettingsPage);
