namespace ContosoUniversityCore.Features.Department
{
    using System.Threading.Tasks;
    using Department;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class DepartmentController : Controller
    {
        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _mediator.Send(new Index.Query());

            return View(model);
        }

        public async Task<ActionResult> Details(Details.Query query)
        {
            var department = await _mediator.Send(query);

            return View(department);
        }

        public ActionResult Create()
        {
            return View(new Create.Command());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Create.Command model)
        {
            await _mediator.Send(model);

            return this.RedirectToActionJson(nameof(Index));
        }

        public async Task<ActionResult> Edit(Edit.Query query)
        {
            var department = await _mediator.Send(query);

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Edit.Command model)
        {
            await _mediator.Send(model);

            return this.RedirectToActionJson(nameof(Index));
        }

        public async Task<ActionResult> Delete(Delete.Query query)
        {
            var model = await _mediator.Send(query);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Delete.Command command)
        {
            await _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }
    }
}