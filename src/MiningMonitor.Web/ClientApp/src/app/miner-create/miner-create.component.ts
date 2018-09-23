import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { Miner } from '../../models/Miner';
import { MinerService } from '../miner.service';

@Component({
    templateUrl: './miner-create.component.html'
})
export class MinerCreateComponent {
    public validationErrors: { [key: string]: string[] } = {};

    constructor(
        private minerService: MinerService,
        private router: Router) {
    }

    public async Save(miner: Miner) {
        try {
            miner = await this.minerService.Create(miner);
            this.router.navigateByUrl(`/miner/${miner.id}`);
        } catch (error) {
            if (error instanceof HttpErrorResponse && error.status === 400) {
                this.validationErrors = error.error;
            }
        }
    }
}
