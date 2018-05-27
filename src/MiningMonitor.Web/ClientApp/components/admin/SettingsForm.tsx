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
                    <Button outline color="info">Save Changes</Button>
                </Form>
            </>
        );
    }
}
