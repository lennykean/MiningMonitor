import * as React from 'react';

import { Button, Col, Form, FormGroup, FormText, Label } from 'reactstrap';

import autobind from 'autobind-decorator';

import { Miner } from '../../models';
import { FormInput } from '../common';

interface Props {
    settings: { [key: string]: string };
    onSubmit?: (settings: { [key: string]: string }) => void;
}

export class SettingsForm extends React.Component<Props, { [key: string]: string }> {
    public constructor(props: Props) {
        super(props);
        this.state = props.settings || {};
    }
    public componentWillReceiveProps(props: Props) {
        if (props === this.props) {
            return;
        }
        this.setState(props.settings);
    }
    @autobind
    public change(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({
            ...this.state,
            [event.target.name]: event.target.type === 'checkbox' ?
                String(event.target.checked) :
                event.target.value,
        });
    }
    @autobind
    public submit(event: React.ChangeEvent<HTMLFormElement>) {
        event.preventDefault();
        if (this.props.onSubmit) {
            this.props.onSubmit(this.state);
        }
    }
    @autobind
    public resetCollector() {
        event.preventDefault();
        if (this.props.onSubmit) {
            this.props.onSubmit({
                ...this.state,
                name: null,
                collectorId: null,
                isDataCollector: null,
                serverToken: null,
                serverUrl: null,
            });
        }
    }
    public render() {
        return (
            <>
                <Form onSubmit={this.submit}>
                    <FormGroup row>
                        <Label for="enableSecurity" sm={2}>Authentication</Label>
                        <Col sm={{ size: 10 }}>
                            <FormGroup check>
                                <Label check>
                                    <FormInput
                                        id="enableSecurity"
                                        name="enableSecurity"
                                        type="checkbox"
                                        checked={this.state.enableSecurity === 'true'}
                                        onChange={this.change}
                                    /> Enabled
                            </Label>
                            </FormGroup>
                        </Col>
                    </FormGroup>
                    <fieldset disabled={this.props.settings && this.props.settings.isDataCollector === 'true'}>
                        <FormGroup row title="Run server in data collector mode">
                            <Label for="isDataCollector" sm={2}>Data Collector</Label>
                            <Col sm={{ size: 10 }}>
                                <FormGroup check>
                                    <Label check>
                                        <FormInput
                                            id="isDataCollector"
                                            name="isDataCollector"
                                            type="checkbox"
                                            checked={this.state.isDataCollector === 'true'}
                                            onChange={this.change}
                                        /> Enabled
                                </Label>
                                </FormGroup>
                            </Col>
                        </FormGroup>
                        <FormGroup row>
                            <Label for="serverUrl" sm={2}>Remote Server URL</Label>
                            <Col sm={{ size: 10 }}>
                                <FormInput
                                    id="serverUrl"
                                    name="serverUrl"
                                    type="text"
                                    value={this.state.serverUrl || ''}
                                    onChange={this.change}
                                />
                            </Col>
                        </FormGroup>
                        <FormGroup row>
                            <Label for="name" sm={2}>Collector Name</Label>
                            <Col sm={{ size: 10 }}>
                                <FormInput
                                    id="name"
                                    name="name"
                                    type="text"
                                    value={this.state.name || ''}
                                    onChange={this.change}
                                />
                            </Col>
                        </FormGroup>
                    </fieldset>
                    {this.props.settings && this.props.settings.isDataCollector === 'true' ?
                        <FormGroup row>
                            <Col sm={2} />
                            <Col sm={{ size: 10 }}>
                                <Button
                                    outline
                                    color="info"
                                    onClick={this.resetCollector}
                                >Reset Collector Settings</Button>
                            </Col>
                        </FormGroup> : null}
                    <Button outline color="info">Save Changes</Button>
                </Form>
            </>
        );
    }
}
