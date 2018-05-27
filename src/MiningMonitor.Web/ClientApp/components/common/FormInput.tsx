import * as React from 'react';

import { Input } from 'reactstrap';

interface Props {
    id: string;
    name?: string;
    type?: 'text' | 'number' | 'password' | 'checkbox';
    value?: string;
    checked?: boolean;
    placeholder?: string;
    validation?: { [key: string]: string[] };
    onChange?: React.ChangeEventHandler<HTMLInputElement>;
}

export const FormInput: React.SFC<Props> = (props) => (
    <>
        <Input
            id={props.id}
            name={props.name}
            type={props.type}
            value={props.value}
            checked={props.checked}
            onChange={props.onChange}
            placeholder={props.placeholder}
            valid={props.validation && props.validation[props.name] && props.validation[props.name].length === 0}
        />
        {props.validation && props.validation[props.name] ?
            <div className="validation-errors">
                {props.validation[props.name].map((message, index) => (
                    <small key={index} className="text-danger">{message}</small>
                ))}
            </div> : null}
    </>
);
