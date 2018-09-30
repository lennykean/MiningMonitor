import { AlertParameters } from './AlertParameters';
import { AlertSeverity } from './AlertSeverity';

export interface AlertDefinition {
    id?: string;
    minerId: string;
    severity?: AlertSeverity;
    displayName: string;
    enabled: boolean;
    parameters: AlertParameters;
    created?: Date;
    updated?: Date;
    lastEnabled?: Date;
    lastScan?: Date;
    name?: string;
}
