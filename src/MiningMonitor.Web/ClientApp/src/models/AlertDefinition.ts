import { AlertParameters } from './AlertParameters';

export interface AlertDefinition {
    id: string;
    minerId: string;
    displayName: string;
    enabled: boolean;
    parameters: AlertParameters;
    created: Date;
    updated: Date;
    lastEnabled: Date;
    lastScan: Date;
    name: string;
}
