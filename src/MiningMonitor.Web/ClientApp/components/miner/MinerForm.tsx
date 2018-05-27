import * as React from 'react';

import { Button, Col, Form, FormGroup, FormText, Label } from 'reactstrap';

import autobind from 'autobind-decorator';

import { Miner } from '../../models';
import { FormInput } from '../common';

interface Props {
    miner?: Miner;
    validation?: { [key: string]: string[] };
    onSubmit?: (miner: Miner) => void;
}

interface State {
    displayName: string;
    address: string;
    port: string;
    collectData: boolean;
}

export class MinerForm extends React.Component<Props, State> {
    public constructor(props: Props) {
        super(props);
        this.state = this.createState(props.miner);
    }
    public componentWillReceiveProps(props: Props) {
        if (props.miner === this.props.miner) {
            return;
        }
        this.setState(this.createState(props.miner));
    }
    @autobind
    public change(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({
            ...this.state,
            [event.target.name]: event.target.type === 'checkbox' ?
                event.target.checked :
                event.target.value,
        });
    }
    @autobind
    public submit(event: React.ChangeEvent<HTMLFormElement>) {
        event.preventDefault();
        if (this.props.onSubmit) {
            this.props.onSubmit({
                ...this.props.miner,
                displayName: this.state.displayName,
                address: this.state.address,
                port: +this.state.port || null,
                collectData: this.state.collectData,
            });
        }
    }
    public render() {
        return (
            <>
                <Form className="miner-form" onSubmit={this.submit}>
                    <fieldset disabled={this.props.miner && !!this.props.miner.collectorId}>
                        <FormGroup row>
                            <Label for="name" sm={2}>
                                Name
                            <small className="form-text text-muted">Optional</small>
                            </Label>
                            <Col sm={10}>
                                <FormInput
                                    id="displayName"
                                    name="displayName"
                                    type="text"
                                    value={this.state.displayName || ''}
                                    onChange={this.change}
                                    placeholder={this.props.miner ? this.props.miner.name : '127.0.0.1:3333'}
                                    validation={this.props.validation}
                                />
                            </Col>
                        </FormGroup>
                        <FormGroup row>
                            <Label for="address" sm={2}>Address</Label>
                            <Col sm={10}>
                                <FormInput
                                    id="address"
                                    name="address"
                                    type="text"
                                    value={this.state.address || ''}
                                    onChange={this.change}
                                    placeholder="127.0.0.1"
                                    validation={this.props.validation}
                                />
                            </Col>
                        </FormGroup>
                        <FormGroup row>
                            <Label for="port" sm={2}>Port</Label>
                            <Col sm={10}>
                                <FormInput
                                    id="port"
                                    name="port"
                                    type="number"
                                    value={this.state.port || ''}
                                    onChange={this.change}
                                    placeholder="3333"
                                    validation={this.props.validation}
                                />
                            </Col>
                        </FormGroup>
                        <FormGroup row>
                            <Label for="collectData" sm={2}>Data Collection</Label>
                            <Col sm={{ size: 10 }}>
                                <FormGroup check>
                                    <Label check>
                                        <FormInput
                                            id="collectData"
                                            name="collectData"
                                            type="checkbox"
                                            checked={this.state.collectData}
                                            onChange={this.change}
                                            validation={this.props.validation}
                                        /> Enabled
                                </Label>
                                </FormGroup>
                            </Col>
                        </FormGroup>
                        {!this.props.miner || !this.props.miner.collectorId ?
                            <Button outline color="info">Save Changes</Button> :
                            null}
                    </fieldset>
                </Form>
            </>
        );
    }
    private createState(miner: Miner) {
        miner = miner || {} as Miner;
        return {
            displayName: miner && miner.displayName,
            address: miner && miner.address,
            port: (miner && miner.port) ? String(miner.port) : null,
            collectData: miner && miner.collectData ? true : false,
        };
    }
}
