<title>D4b · Where carried platform info lives relative to the PSG</title>

<article class="doc">

<header class="masthead">
  <div class="masthead-rail">
    <span class="kicker">Fidelity.Platform · decision-support brief</span>
    <span class="kicker kicker-dim">companion to Canonical Spec · D4</span>
  </div>
  <h1>Where carried platform information lives<br>relative to the Program Semantic Graph</h1>
  <p class="standfirst">
    D4 settled that platform facts are carried <em>open</em>, not flattened to scalars early.
    This brief resolves the sub-question D4 left open: does the carried structure sit
    <span class="tag tag-a">aside</span> the PSG, consulted as a context, or ride as a
    <span class="tag tag-b">head node</span> the zipper witnesses? Tradeoffs and stress cases,
    grounded in the shipping code. No recommendation — the decision is the architect&rsquo;s.
  </p>
  <dl class="meta-grid">
    <div><dt>Status</dt><dd>Open · for architect decision</dd></div>
    <div><dt>Settled upstream</dt><dd>Carry open, concretize at the latest responsible moment</dd></div>
    <div><dt>Discriminator</dt><dd>The unified cache&nbsp;+&nbsp;concurrency braid solve</dd></div>
    <div><dt>Grounded in</dt><dd>clef&nbsp;@&nbsp;fidelity · PSGSaturation, Baker</dd></div>
  </dl>
</header>

<nav class="legend" aria-label="Option colour key">
  <span class="legend-item"><span class="swatch swatch-a"></span><b>Option A — Aside.</b> A quotation/dictionary that sits beside the graph; passes consult it as a context.</span>
  <span class="legend-item"><span class="swatch swatch-b"></span><b>Option B — Head node.</b> A node in the graph (root or near it); the zipper witnesses it in normal traversal.</span>
  <span class="legend-item"><span class="swatch swatch-c"></span><b>Third reading.</b> In-graph hyperedge annotations — neither aside nor a head node. The sources keep pointing here.</span>
</nav>

<!-- ================= 01 ================= -->
<section class="band">
  <div class="band-head">
    <span class="num">01</span>
    <h2>What the architecture already leans toward</h2>
  </div>

  <p class="lede">
    The strongest input to <em>less-inviting-of-rework</em> is not philosophy — it is what the code already is.
    Read against the shipping tree, the answer is unambiguous: <strong>today leans Aside</strong>, and there is
    <strong>no head-node precedent anywhere</strong>.
  </p>

  <div class="finding">
    <h3>The graph is a forest, not an apex. Platform is a field beside the nodes.</h3>
    <p>
      <code>SemanticGraph</code> is a node <em>map</em> plus a <em>list</em> of declaration roots. Platform is a
      sibling field of <code>Nodes</code>, never a member of it:
    </p>
<pre class="code"><span class="c-dim">// Types.fs:379–388</span>
[&lt;NoComparison; NoEquality&gt;]
type SemanticGraph = {
    Nodes: Map&lt;NodeId, SemanticNode&gt;
    DeclarationRoots: (NodeId * DeclRoot) list      <span class="c-b">// a LIST — a forest, no single head</span>
    Modules: Map&lt;ModulePath, NodeId list&gt;
    Types: Lazy&lt;Map&lt;string, NodeId&gt;&gt;
    Platform: PlatformContext option                <span class="c-a">// platform lives HERE, aside the nodes</span>
    ModuleClassifications: Lazy&lt;Map&lt;NodeId, ModuleClassification&gt;&gt;
    SeqSaturation: Lazy&lt;Map&lt;NodeId, SeqStateMachineInfo&gt;&gt;
}</pre>
    <p>
      There is no root node to hang platform on. <code>DeclRoot</code> is <code>EntryPoint | HardwareModule | KernelModule</code>,
      and its own doc-comment states the pipeline&rsquo;s posture outright:
    </p>
    <blockquote class="src">
      &ldquo;Platform-agnostic: the pipeline routes based on <code>DeclRoot</code> kind.&rdquo;
      <cite>Types.fs:142</cite>
    </blockquote>
    <p>Each root is a co-equal, platform-agnostic entry point. None carries platform facts.</p>
  </div>

  <div class="finding">
    <h3>Every traversal seeds from the root list. The walk never witnesses <code>Platform</code>.</h3>
    <p>All four folds end with the identical line — the roots <em>are</em> the declaration roots, and nothing else:</p>
<pre class="code"><span class="c-dim">// Traversal.fs — lines 59, 72, 126, 279, identically</span>
graph.DeclarationRoots |&gt; List.map fst |&gt; List.fold walk state</pre>
    <p>
      <code>walk</code> descends only via <code>getSemanticReferences</code> + <code>node.Children</code>. The
      <code>Platform</code> field is invisible to the zipper. This is the passive-zipper contract:
      &ldquo;the zipper witnesses, it does not decide&rdquo; <span class="cite-inline">learning-to-walk.md:102</span>.
    </p>
  </div>

  <div class="finding">
    <h3>Every consumer takes platform as a parameter <em>beside</em> the graph — never reads it from a node.</h3>
    <p>
      The one shipping machine-analysis consumer is the template. Platform is the <em>first argument</em>; the
      graph is the second; the walk that follows never sees platform:
    </p>
<pre class="code"><span class="c-dim">// DepthAnalysis.fs:224–235 — the FPGA combinational-depth solve</span>
let analyze (platformContext: PlatformContext option) (graph: SemanticGraph) =
    match platformContext with
    | Some ctx when PlatformContext.substrateKind ctx = SubstrateKind.FPGA -&gt;
        let threshold = computeThreshold ctx.ClockFrequencyMhz ctx.NsPerWeightUnit
        Traversal.foldWithLambdaPreBind (fun s _ -&gt; s) analyzeNode (state ctx) graph
    | _ -&gt; []          <span class="c-a">// aside GATE: decided before any walk</span></pre>
    <p>
      Baker threads it the same way, as a reader over parser state — <code>SaturationState.Platform</code>, pulled
      mid-solve via <code>getPlatform</code> <span class="cite-inline">SaturationCombinators.fs:41,127</span> — and
      hand-copies <code>Platform = ctx.Platform</code> down through every recipe context. It is plumbed as a
      parameter pipeline, never as a walked node.
    </p>
  </div>

  <div class="callout callout-neutral">
    <span class="callout-label">Net for rework exposure</span>
    <p>
      <span class="tag tag-a">Aside</span> is the status quo: the field, the constructors
      (<code>withPlatform</code>, <code>BuildWithPlatform</code>), and consumers-as-parameters all exist and work.
      <span class="tag tag-b">Head node</span> is entirely new structure — a new <code>SemanticKind</code> case, a
      seeding change so the walk reaches it, and real edges so the fold descends through it. The lean toward Aside
      is a fact about the code, not a preference.
    </p>
  </div>
</section>

<!-- ================= 02 ================= -->
<section class="band">
  <div class="band-head">
    <span class="num">02</span>
    <h2>The tradeoff map</h2>
  </div>
  <p class="lede">
    Six axes that actually decide this. Each cell is a claim about <em>this</em> design, grounded, not abstract.
  </p>

  <div class="table-scroll">
  <table class="tradeoff">
    <thead>
      <tr>
        <th class="axis-col">Axis</th>
        <th class="opt-a">Option A — Aside</th>
        <th class="opt-b">Option B — Head node</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td class="axis">
          <b>Traversal ergonomics</b>
          <span class="axis-sub">Is the fact in scope when a pass needs it — including mid-traversal?</span>
        </td>
        <td class="opt-a">
          In scope <em>by convention</em>: captured into fold <code>'State</code> or read via <code>getPlatform</code>.
          Available anywhere a pass author remembered to thread it. Nothing structural keeps it in scope at a deep focus.
        </td>
        <td class="opt-b">
          In scope <em>structurally</em> — but only in the true-apex variant (B2), where <code>node.Parent</code> climbing
          reaches platform from any focus with zero threading. The cheap variant (B1) just relocates the thread to
          &ldquo;first fold step&rdquo; and buys nothing the reader monad didn&rsquo;t.
        </td>
      </tr>
      <tr>
        <td class="axis">
          <b>Target-conditional elision</b>
          <span class="axis-sub">How does cache / deadlock analysis vanish on a thin target?</span>
        </td>
        <td class="opt-a">
          A <em>gate</em>: <code>match platform with FPGA -&gt; … | _ -&gt; []</code>. A per-pass boolean, decided
          before the walk. Correct, but invites N independent gates for a solve that is supposed to be one.
        </td>
        <td class="opt-b">
          A <em>witnessed thin ground</em>: the solve reads a single-tier / single-core payload at the focus and the
          coloring falls out as &ldquo;one color.&rdquo; The simplification is structure, not a short-circuit — <em>if</em>
          you pay for B2.
        </td>
      </tr>
      <tr>
        <td class="axis">
          <b>Single source of truth</b>
          <span class="axis-sub">Deferred concretization writing back into placement.</span>
        </td>
        <td class="opt-a">
          Clean for <em>target-given, read-only</em> facts. Strains when the braid <em>writes</em>: a stale concretized
          view has no timestamp, and tile-global running budgets the solve threads are neither node-local nor
          target-given — they fall between the graph and the aside. No graph&nbsp;&rarr;&nbsp;aside arrow exists.
        </td>
        <td class="opt-b">
          Concretization writes into the graph the node lives in, so placement results have a home. But a single apex
          holds <em>global</em> capability, while the braid&rsquo;s write-back is <em>per-locus</em> (this tile, this
          crossing) — the head node is the wrong shape for the write, even if it owns the read.
        </td>
      </tr>
      <tr>
        <td class="axis">
          <b>Coupling</b>
          <span class="axis-sub">Do passes couple to platform shape? Does every node depend on the root?</span>
        </td>
        <td class="opt-a">
          Coupling is opt-in: a pass that ignores platform never mentions it. No universal dependency. The cost of that
          looseness is that a pass can silently <em>forget</em> to consult it.
        </td>
        <td class="opt-b">
          Under B2, every focus may climb to the apex, so every node is implicitly platform-relative and every trivial
          CPU hello-world must grow and carry a head. Coupling becomes universal — a virtue (can&rsquo;t forget) or a
          tax (everyone pays), depending on how you read it.
        </td>
      </tr>
      <tr>
        <td class="axis">
          <b>Extensibility</b>
          <span class="axis-sub">Adding a new conditional consumer / capability dimension.</span>
        </td>
        <td class="opt-a">
          Cheapest path. A new <em>global</em> dimension is one record field + one query; passes that don&rsquo;t care
          never mention it. No exhaustiveness obligation is triggered.
        </td>
        <td class="opt-b">
          A new global field is as cheap as A. But a new <code>SemanticKind</code> case pays the exhaustiveness tax —
          an arm in <em>every</em> no-wildcard match (<code>getSemanticReferences</code>, SCF traversal, emission
          dispatch), a discipline the codebase deliberately enforces.
        </td>
      </tr>
      <tr>
        <td class="axis">
          <b>Rework exposure</b>
          <span class="axis-sub">Given what exists today.</span>
        </td>
        <td class="opt-a">
          Near-zero: it is the shipping shape. The one honest debt is that <code>PlatformContext</code> is today
          <em>authored</em>, not <em>derived</em> from the open <code>MemoryModel</code> carrier — so &ldquo;carry
          open&rdquo; is partly aspirational until the open <code>Expr</code> fields actually reach the graph object.
        </td>
        <td class="opt-b">
          High and structural: touches all four traversal folds, reachability seeding, root discovery, and every
          exhaustive match. Buys real structural scope only in the expensive variant; the cheap variant collapses
          back into A.
        </td>
      </tr>
    </tbody>
  </table>
  </div>
</section>

<!-- ================= 03 ================= -->
<section class="band">
  <div class="band-head">
    <span class="num">03</span>
    <h2>Stress cases, side by side</h2>
  </div>
  <p class="lede">
    Framed against the <em>unified</em> braid solve — region classification, lane/layout selection, and crossing
    coordination as one capability-driven solve over the graph — not against isolated passes.
  </p>

  <div class="table-scroll">
  <table class="stress">
    <thead>
      <tr>
        <th class="case-col">Stress case</th>
        <th class="opt-a">Aside handles it&hellip;</th>
        <th class="opt-b">Head node handles it&hellip;</th>
        <th class="strain-col">Where each strains</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <td class="case">
          <b>Single-core ARM</b><br><span class="case-sub">the braid should <em>simplify</em></span>
        </td>
        <td class="opt-a">Read the flat ground up front, emit a trivial obligation. Clean — the ground is small and read once.</td>
        <td class="opt-b">Witness the thin payload at the focus; coloring resolves to one color locally. Elegant in B2.</td>
        <td class="strain">
          <span class="s-a">A</span> invites two gates (no-cache, single-core) instead of one thin solve — fragmenting
          &ldquo;one solve.&rdquo;
          <span class="s-b">B</span> is least load-bearing here: the real signal is <em>absent wait-for edges</em>, which
          need no platform node at all.
        </td>
      </tr>
      <tr>
        <td class="case">
          <b>FPGA / multi-tier</b><br><span class="case-sub">the braid needs <em>everything</em></span>
        </td>
        <td class="opt-a">The rich menu lives in a record-of-maps-and-<code>Expr</code>; <code>DepthAnalysis</code> already computes a threshold from it.</td>
        <td class="opt-b">The menu is reachable by climbing — but it is <em>global</em> menu, not <em>this crossing&rsquo;s</em> tile.</td>
        <td class="strain">
          <span class="s-a">A</span> joins two objects (graph + aside) <em>per crossing</em>; the &ldquo;one object with
          crossings intact&rdquo; is physically two the author keeps re-marrying.
          <span class="s-b">B</span> answers &ldquo;where is platform&rdquo; when the solve asks &ldquo;what governs
          <em>here</em>&rdquo; — the deepest strain.
        </td>
      </tr>
      <tr>
        <td class="case">
          <b>Mid-traversal query</b><br><span class="case-sub">a capability needed deep in the graph</span>
        </td>
        <td class="opt-a">Always available via closure / <code>getPlatform</code>. In scope by convention, not by structure.</td>
        <td class="opt-b">B2&rsquo;s best case: <code>node.Parent</code> climbing reaches a global capability from any depth, no threading.</td>
        <td class="strain">
          <span class="s-a">A</span> &ldquo;in scope&rdquo; depends on every intervening author threading it —
          forgetting yields a wrong-but-valid flat answer.
          <span class="s-b">B</span> the witnessed head is <em>inert</em> (passive zipper): it buys fact-reachability,
          not decision-making; climbing is O(depth) unless you rebuild an aside cache.
        </td>
      </tr>
      <tr>
        <td class="case">
          <b>New consumer</b><br><span class="case-sub">add a capability dimension later</span>
        </td>
        <td class="opt-a">One field, one query. Passes that don&rsquo;t care never change. The clean win.</td>
        <td class="opt-b">Cheap if the dimension is a global field; costly if it is per-locus (pushes toward hyperedges anyway).</td>
        <td class="strain">
          <span class="s-a">A</span> the same non-witnessing that makes adding free lets a pass <em>forget</em> an
          existing dimension.
          <span class="s-b">B</span> a per-locus dimension reveals the single apex was the wrong bet — migrate to
          edge-borne annotations.
        </td>
      </tr>
    </tbody>
  </table>
  </div>

  <div class="callout callout-c">
    <span class="callout-label">The tell that recurs in every hard case</span>
    <p>
      Rows 1, 2 and 4 all push the same direction. The braid&rsquo;s load-bearing facts — which tile governs a region,
      which coherency domain a crossing crosses, which tier an arena colors against — are <strong>joint constraints
      local to crossings</strong>, and <em>Weaving the Braid</em> already proposes to carry them as
      <span class="tag tag-c">in-graph hyperedge annotations</span>: &ldquo;co-location of several values on one
      hardware tile is a meaningful claim of this &lsquo;joint&rsquo; kind&hellip; the crossings of the braid are
      <em>exactly</em> the relationship the hyperedge form exists to hold.&rdquo; That is neither a table beside the
      graph nor a node atop it. It is a genuine third location the two-way fork doesn&rsquo;t name — and it is where
      the braid&rsquo;s own prose points.
    </p>
  </div>
</section>

<!-- ================= 04 ================= -->
<section class="band">
  <div class="band-head">
    <span class="num">04</span>
    <h2>What deferred inference itself implies about location</h2>
  </div>

  <div class="two-col">
    <div>
      <h3>Be honest: the philosophy is largely <em>silent</em> on location.</h3>
      <p>
        The Gift of Deferred Inference constrains <em>when</em> and <em>how</em> a fact concretizes — carry the option
        space open, concretize at the latest responsible moment, by whichever stage has the most information. Those are
        claims about <em>timing and openness</em>, and they are fully satisfiable in <em>either</em> carrier: an
        <code>Expr</code>-valued field decomposes by pattern just as well aside as on a node. Openness is orthogonal to
        node-vs-aside. Anyone who tells you deferred inference <em>mandates</em> a location is over-reading it.
      </p>
    </div>
    <div>
      <h3>Where it does lean, it leans faintly toward <em>local</em>.</h3>
      <p>
        There is one soft pull. If concretization is a write that lands at the <em>latest responsible</em> point in the
        PSG, and that point is a node the pass is focused on, then the ground the write is computed against being
        <em>reachable from that focus</em> keeps concretization local — no reach-outside at the moment of commitment.
        That argues for the facts being <em>in the graph the concretization passes walk</em>. But note it argues for
        <span class="tag tag-c">edge-borne, at the locus</span> at least as strongly as for a
        <span class="tag tag-b">single head node</span> — because the write is per-locus, and a global apex is not
        local to it.
      </p>
    </div>
  </div>

  <div class="callout callout-neutral">
    <span class="callout-label">The principled residue</span>
    <p>
      Deferred inference does not choose Aside vs Head node. What it does say is: the ground a concretization solves
      <em>against</em> should be present <em>where and when</em> the commitment is made. On simple targets that is
      trivially satisfied by any carrier. It only bites where the commitment is per-crossing and capability-dependent —
      and there it favors the ground being <em>in the graph, at the locus</em>, over both a distant table and a distant
      apex. That is a statement about <em>the braid</em>, not about platform-in-general.
    </p>
  </div>
</section>

<!-- ================= 05 ================= -->
<section class="band band-decision">
  <div class="band-head">
    <span class="num">05</span>
    <h2>The decision as it actually stands</h2>
  </div>

  <p class="lede decision-lede">
    Stated crisply, so it is sharp enough to decide. This is not &ldquo;aside vs node&rdquo; in the abstract — it is a
    choice about <em>one specific solve</em>.
  </p>

  <div class="choice-grid">
    <div class="choice choice-a">
      <span class="choice-tag">Choose Aside if</span>
      <ul>
        <li>Platform stays what it is <em>today</em>: target-given, whole-graph, read-only — a context many nodes consume.</li>
        <li>Drift is controllable — you commit to deriving <code>PlatformContext</code> as a <code>Lazy</code> projection of the open carrier, not authoring it twice.</li>
        <li>You accept a per-crossing join at the braid, maintained by discipline rather than guaranteed by structure.</li>
      </ul>
      <p class="commits">Commits you to: zero structural rework; a convention that every braid pass threads the ground and never forgets; and no home for solve-derived, tile-global state — you find one <em>later</em>, when the braid writes.</p>
    </div>
    <div class="choice choice-b">
      <span class="choice-tag">Choose Head node if</span>
      <ul>
        <li>Passes commonly need a <em>global</em> capability <em>mid-traversal, deep in the graph</em>, and threading it everywhere is the real pain you are buying out.</li>
        <li>You want the target-conditional simplification to read as witnessed thin ground, not as a scatter of per-pass gates.</li>
        <li>You are willing to pay the exhaustiveness tax and rewrite the four folds for a real (B2) apex — not the B1 shape that collapses into Aside.</li>
      </ul>
      <p class="commits">Commits you to: new <code>SemanticKind</code>, new seeding, universal root-coupling — for a global-read affordance that is still <em>inert</em> under the passive zipper, and still the wrong shape for per-locus writes.</p>
    </div>
  </div>

  <div class="tip">
    <h3>The one or two facts that tip it</h3>
    <p>
      <b>If braid passes commonly need platform facts <em>mid-traversal at a deep focus</em></b> and the fact they need
      is <em>global</em> (a target predicate like <code>has_neon</code>, a line size), the head node&rsquo;s
      climb-to-root is the only thing that gives that structurally &mdash; that tips toward
      <span class="tag tag-b">B</span>.
    </p>
    <p>
      <b>If the aside already works, drift is controllable, and the facts the braid needs are per-crossing</b>
      (tile co-location, coherency domain, wait-for rank), then neither the table nor the apex fits the write, and the
      pressure is not toward B at all &mdash; it is toward <span class="tag tag-c">hyperedge annotations in the graph</span>,
      with the aside kept for the genuinely global, read-only, target-given ground. In that reading the fork itself is
      mis-posed: keep <span class="tag tag-a">A</span> for what platform <em>is</em>, and let the braid&rsquo;s
      <em>own</em> facts ride the graph as edges &mdash; not as a head node.
    </p>
    <p class="tip-sharp">
      The sharpest single test: point at the <em>one</em> QF-LIA obligation where cache-tier, tile-topology, interval
      bound and wait-for rank are discharged together. Ask which carrier lets that one obligation see all four
      <em>without a side-channel join</em>. Whatever answers that is what the braid is actually asking for. Everything
      else is ergonomics.
    </p>
  </div>
</section>

<footer class="colophon">
  <div class="colophon-row">
    <span>Decision-support brief · no recommendation by design</span>
    <span>Companion to <code>CANONICAL_PLATFORM_SPEC.md</code> · D4</span>
  </div>
  <p class="sources">
    Grounded in: <code>PSGSaturation/SemanticGraph/{Types,Traversal,Reachability,Core,Builder,DepthAnalysis}.fs</code>
    · <code>Baker/Ingredients/SaturationCombinators.fs</code> · <code>NativeTypedTree/NativeTypes.fs</code> ·
    docs <code>weaving-the-braid</code>, <code>learning-to-walk</code>, <code>deadlock-freedom-as-an-obligation</code>,
    <code>on-metal-revisited</code>, <code>cache-aware-compilation-{cpu,gpu}</code>,
    <code>context-aware-compilation</code>.
  </p>
</footer>

</article>

<style>
  :root {
    /* cool slate-biased neutrals — a graph/blueprint world, not warm cream */
    --paper:      #f4f6f8;
    --paper-2:    #eceff3;
    --card:       #ffffff;
    --ink:        #1a1f2b;
    --ink-2:      #3b4353;
    --slate:      #5b6472;
    --slate-dim:  #828b99;
    --rule:       #d6dbe2;
    --rule-soft:  #e4e8ee;

    /* option-coded accents — the one information-carrying colour device */
    --a:          #1f7a72;   /* Aside — muted teal */
    --a-tint:     #e2f0ee;
    --a-line:     #9fccc6;
    --b:          #b0742a;   /* Head node — muted ochre */
    --b-tint:     #f3e9da;
    --b-line:     #ddb887;
    --c:          #6a5aa6;   /* Third reading — muted violet */
    --c-tint:     #eae7f3;
    --c-line:     #bcb2dc;
    --strain:     #a8503f;   /* semantic: where it strains */

    --code-bg:    #1c2230;
    --code-ink:   #d7dce6;
    --code-dim:   #7d8798;

    --serif: "Iowan Old Style","Palatino Linotype",Palatino,"Source Serif 4",Charter,Georgia,"Times New Roman",serif;
    --sans:  "Inter var",Inter,-apple-system,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif;
    --mono:  "JetBrains Mono","JetBrainsMono Nerd Font Mono","SFMono-Regular",ui-monospace,"Cascadia Mono",Menlo,Consolas,monospace;

    --measure: 68ch;
    --maxw: 78rem;
  }

  @media (prefers-color-scheme: dark) {
    :root {
      --paper:     #12151c;
      --paper-2:   #171b24;
      --card:      #1a1f29;
      --ink:       #eef1f6;
      --ink-2:     #c3cad6;
      --slate:     #98a2b2;
      --slate-dim: #6f7889;
      --rule:      #2b323f;
      --rule-soft: #232935;

      --a:      #4fb8ae; --a-tint: #16302e; --a-line: #2f6b64;
      --b:      #d69a52; --b-tint: #33281a; --b-line: #7a5a30;
      --c:      #a396d8; --c-tint: #241f36; --c-line: #4b4173;
      --strain: #d98a76;

      --code-bg: #0d1017;
      --code-ink: #cdd4df;
      --code-dim: #6d7789;
    }
  }
  :root[data-theme="light"] {
    --paper:#f4f6f8; --paper-2:#eceff3; --card:#ffffff; --ink:#1a1f2b; --ink-2:#3b4353;
    --slate:#5b6472; --slate-dim:#828b99; --rule:#d6dbe2; --rule-soft:#e4e8ee;
    --a:#1f7a72; --a-tint:#e2f0ee; --a-line:#9fccc6;
    --b:#b0742a; --b-tint:#f3e9da; --b-line:#ddb887;
    --c:#6a5aa6; --c-tint:#eae7f3; --c-line:#bcb2dc; --strain:#a8503f;
    --code-bg:#1c2230; --code-ink:#d7dce6; --code-dim:#7d8798;
  }
  :root[data-theme="dark"] {
    --paper:#12151c; --paper-2:#171b24; --card:#1a1f29; --ink:#eef1f6; --ink-2:#c3cad6;
    --slate:#98a2b2; --slate-dim:#6f7889; --rule:#2b323f; --rule-soft:#232935;
    --a:#4fb8ae; --a-tint:#16302e; --a-line:#2f6b64;
    --b:#d69a52; --b-tint:#33281a; --b-line:#7a5a30;
    --c:#a396d8; --c-tint:#241f36; --c-line:#4b4173; --strain:#d98a76;
    --code-bg:#0d1017; --code-ink:#cdd4df; --code-dim:#6d7789;
  }

  * { box-sizing: border-box; }

  body {
    margin: 0;
    background: var(--paper);
    color: var(--ink);
    font-family: var(--serif);
    font-size: 18px;
    line-height: 1.62;
    -webkit-font-smoothing: antialiased;
    text-rendering: optimizeLegibility;
  }

  .doc {
    max-width: var(--maxw);
    margin: 0 auto;
    padding: clamp(1.4rem, 4vw, 3.5rem) clamp(1.1rem, 4vw, 3rem) 4rem;
  }

  code {
    font-family: var(--mono);
    font-size: 0.83em;
    background: var(--paper-2);
    border: 1px solid var(--rule-soft);
    border-radius: 4px;
    padding: 0.06em 0.34em;
    color: var(--ink-2);
    word-break: break-word;
  }

  /* ---------- masthead ---------- */
  .masthead { border-bottom: 2px solid var(--ink); padding-bottom: 1.9rem; margin-bottom: 2.6rem; }
  .masthead-rail {
    display: flex; flex-wrap: wrap; gap: 0.6rem 1.4rem;
    align-items: baseline; margin-bottom: 1.5rem;
  }
  .kicker {
    font-family: var(--mono);
    font-size: 0.7rem; letter-spacing: 0.16em; text-transform: uppercase;
    color: var(--a); font-weight: 600;
  }
  .kicker-dim { color: var(--slate-dim); font-weight: 500; }
  .masthead h1 {
    font-family: var(--serif);
    font-weight: 600; font-size: clamp(1.9rem, 4.8vw, 3.25rem);
    line-height: 1.08; letter-spacing: -0.014em; margin: 0 0 1.2rem;
    text-wrap: balance; color: var(--ink);
  }
  .standfirst {
    font-size: clamp(1.02rem, 1.9vw, 1.22rem);
    line-height: 1.56; color: var(--ink-2);
    max-width: 54ch; margin: 0 0 1.9rem;
  }
  .standfirst em { font-style: italic; color: var(--ink); }

  .meta-grid {
    display: grid; grid-template-columns: repeat(auto-fit, minmax(11rem, 1fr));
    gap: 1px; margin: 0; background: var(--rule);
    border: 1px solid var(--rule); border-radius: 8px; overflow: hidden;
  }
  .meta-grid > div { background: var(--card); padding: 0.85rem 1.05rem; }
  .meta-grid dt {
    font-family: var(--mono); font-size: 0.64rem; letter-spacing: 0.13em;
    text-transform: uppercase; color: var(--slate-dim); margin: 0 0 0.28rem;
  }
  .meta-grid dd { margin: 0; font-size: 0.94rem; color: var(--ink); line-height: 1.35; }

  /* ---------- legend ---------- */
  .legend {
    display: grid; gap: 0.7rem;
    background: var(--paper-2);
    border: 1px solid var(--rule);
    border-radius: 10px;
    padding: 1.1rem 1.3rem;
    margin-bottom: 3.2rem;
    font-family: var(--sans);
    font-size: 0.9rem; line-height: 1.5; color: var(--ink-2);
  }
  .legend-item { display: grid; grid-template-columns: auto 1fr; gap: 0.7rem; align-items: start; }
  .legend-item b { color: var(--ink); font-weight: 650; }
  .swatch { width: 0.95rem; height: 0.95rem; border-radius: 3px; margin-top: 0.22rem; }
  .swatch-a { background: var(--a); }
  .swatch-b { background: var(--b); }
  .swatch-c { background: var(--c); }

  /* inline option tags */
  .tag {
    font-family: var(--mono); font-size: 0.74em; font-weight: 600;
    letter-spacing: 0.02em; padding: 0.08em 0.42em; border-radius: 4px;
    white-space: nowrap; border: 1px solid transparent;
  }
  .tag-a { color: var(--a); background: var(--a-tint); border-color: var(--a-line); }
  .tag-b { color: var(--b); background: var(--b-tint); border-color: var(--b-line); }
  .tag-c { color: var(--c); background: var(--c-tint); border-color: var(--c-line); }

  /* ---------- bands ---------- */
  .band { margin-bottom: 3.6rem; }
  .band-head {
    display: grid; grid-template-columns: auto 1fr; align-items: baseline;
    gap: 1.1rem; margin-bottom: 1.7rem;
    padding-bottom: 0.7rem; border-bottom: 1px solid var(--rule);
  }
  .num {
    font-family: var(--mono); font-size: 1.05rem; font-weight: 700;
    color: var(--a); letter-spacing: 0.02em;
  }
  .band-head h2 {
    font-family: var(--serif); font-weight: 600;
    font-size: clamp(1.35rem, 2.8vw, 1.85rem); letter-spacing: -0.01em;
    margin: 0; line-height: 1.15; text-wrap: balance; color: var(--ink);
  }
  .lede { font-size: 1.06rem; color: var(--ink-2); max-width: var(--measure); margin: 0 0 1.7rem; }
  .lede strong { color: var(--ink); font-weight: 650; }
  .lede em { font-style: italic; }

  p { max-width: var(--measure); }
  .finding p, .two-col p { color: var(--ink-2); }

  /* ---------- findings ---------- */
  .finding {
    margin-bottom: 1.9rem; padding-left: 1.2rem;
    border-left: 3px solid var(--rule);
  }
  .finding h3 {
    font-family: var(--sans); font-weight: 650; font-size: 1.04rem;
    letter-spacing: -0.005em; color: var(--ink); margin: 0 0 0.7rem; line-height: 1.35;
    max-width: var(--measure); text-wrap: balance;
  }
  .finding h3 em { color: var(--a); font-style: normal; }

  blockquote.src {
    font-family: var(--serif); font-style: italic;
    color: var(--ink); background: var(--paper-2);
    border-left: 3px solid var(--a); border-radius: 0 6px 6px 0;
    margin: 1rem 0; padding: 0.8rem 1.1rem; font-size: 0.98rem;
    max-width: var(--measure);
  }
  blockquote.src cite {
    display: block; font-family: var(--mono); font-style: normal;
    font-size: 0.72rem; letter-spacing: 0.04em; color: var(--slate-dim);
    margin-top: 0.5rem;
  }
  blockquote.src code { background: transparent; border: none; padding: 0; font-style: normal; }

  .cite-inline {
    font-family: var(--mono); font-size: 0.76em; color: var(--slate-dim);
    white-space: nowrap;
  }

  /* ---------- code blocks ---------- */
  pre.code {
    font-family: var(--mono); font-size: 0.8rem; line-height: 1.6;
    background: var(--code-bg); color: var(--code-ink);
    border-radius: 9px; padding: 1.05rem 1.2rem;
    overflow-x: auto; margin: 1rem 0 1.2rem;
    border: 1px solid rgba(255,255,255,0.06);
    -webkit-font-smoothing: auto;
  }
  pre.code .c-dim { color: var(--code-dim); }
  pre.code .c-a { color: #6fd0c6; }
  pre.code .c-b { color: #e6b06a; }

  /* ---------- callouts ---------- */
  .callout {
    border-radius: 10px; padding: 1.15rem 1.35rem; margin: 1.8rem 0 0;
    border: 1px solid var(--rule); background: var(--card); max-width: 100%;
  }
  .callout p { max-width: 62ch; margin: 0; color: var(--ink-2); font-size: 1rem; }
  .callout-label {
    display: block; font-family: var(--mono); font-size: 0.66rem;
    letter-spacing: 0.14em; text-transform: uppercase; font-weight: 600;
    margin-bottom: 0.55rem;
  }
  .callout-neutral { border-left: 4px solid var(--slate); }
  .callout-neutral .callout-label { color: var(--slate); }
  .callout-c { border-left: 4px solid var(--c); background: var(--c-tint); }
  .callout-c .callout-label { color: var(--c); }
  .callout-c p { color: var(--ink); }
  .callout p strong { color: var(--ink); font-weight: 650; }
  .callout em { font-style: italic; }

  /* ---------- tables ---------- */
  .table-scroll { overflow-x: auto; margin: 0 0 0.5rem; border-radius: 10px; }
  table { border-collapse: collapse; width: 100%; min-width: 46rem; font-family: var(--sans); }
  table th, table td { text-align: left; vertical-align: top; padding: 0.9rem 1.05rem; }
  thead th {
    font-family: var(--mono); font-size: 0.72rem; letter-spacing: 0.06em;
    text-transform: uppercase; font-weight: 600;
    border-bottom: 2px solid var(--ink); background: var(--paper-2);
    position: sticky; top: 0;
  }
  tbody tr { border-bottom: 1px solid var(--rule); }
  tbody tr:last-child { border-bottom: none; }
  table td { font-size: 0.92rem; line-height: 1.5; color: var(--ink-2); }
  table code { font-size: 0.8em; }

  /* option column tinting — the consistent A/B code */
  th.opt-a, th.opt-b { border-bottom-width: 2px; }
  th.opt-a { color: var(--a); box-shadow: inset 0 -3px 0 var(--a); }
  th.opt-b { color: var(--b); box-shadow: inset 0 -3px 0 var(--b); }
  .tradeoff td.opt-a, .stress td.opt-a { background: color-mix(in srgb, var(--a-tint) 42%, transparent); }
  .tradeoff td.opt-b, .stress td.opt-b { background: color-mix(in srgb, var(--b-tint) 42%, transparent); }
  td.opt-a em, td.opt-b em { font-style: italic; color: var(--ink); }

  td.axis, td.case { color: var(--ink); }
  td.axis b, td.case b { font-family: var(--sans); font-weight: 650; font-size: 0.98rem; display: block; }
  .axis-sub, .case-sub {
    display: block; font-size: 0.82rem; color: var(--slate-dim);
    margin-top: 0.3rem; line-height: 1.4; font-style: italic;
  }
  .axis-col { width: 20%; } .opt-a, .opt-b { width: 27%; }
  .stress .case-col { width: 15%; } .stress .strain-col { width: 31%; }

  td.strain { color: var(--ink-2); font-size: 0.89rem; }
  .s-a, .s-b {
    display: inline-block; font-family: var(--mono); font-weight: 700;
    font-size: 0.72rem; width: 1.25rem; height: 1.25rem; line-height: 1.25rem;
    text-align: center; border-radius: 4px; margin-right: 0.2rem;
  }
  .s-a { color: var(--a); background: var(--a-tint); border: 1px solid var(--a-line); }
  .s-b { color: var(--b); background: var(--b-tint); border: 1px solid var(--b-line); }
  td.strain em { font-style: italic; color: var(--ink); }

  /* ---------- two-col ---------- */
  .two-col { display: grid; grid-template-columns: 1fr 1fr; gap: 1.8rem 2.4rem; margin-bottom: 0.5rem; }
  .two-col h3 {
    font-family: var(--sans); font-weight: 650; font-size: 1.02rem;
    color: var(--ink); margin: 0 0 0.6rem; line-height: 1.3; text-wrap: balance;
  }
  .two-col h3 em { color: var(--ink); font-style: italic; }
  .two-col p { max-width: none; font-size: 0.98rem; margin: 0; }

  /* ---------- decision ---------- */
  .band-decision .num { color: var(--b); }
  .decision-lede em { font-style: italic; color: var(--ink); }

  .choice-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 1.4rem; margin: 0 0 2rem; }
  .choice {
    border-radius: 12px; padding: 1.3rem 1.4rem; background: var(--card);
    border: 1px solid var(--rule);
  }
  .choice-a { border-top: 4px solid var(--a); }
  .choice-b { border-top: 4px solid var(--b); }
  .choice-tag {
    display: inline-block; font-family: var(--mono); font-size: 0.7rem;
    letter-spacing: 0.08em; text-transform: uppercase; font-weight: 600;
    margin-bottom: 0.9rem;
  }
  .choice-a .choice-tag { color: var(--a); }
  .choice-b .choice-tag { color: var(--b); }
  .choice ul { margin: 0 0 1rem; padding-left: 1.15rem; }
  .choice li { font-family: var(--sans); font-size: 0.92rem; line-height: 1.5; color: var(--ink-2); margin-bottom: 0.6rem; }
  .choice li em { font-style: italic; color: var(--ink); }
  .commits {
    font-family: var(--sans); font-size: 0.86rem; line-height: 1.5;
    color: var(--ink); margin: 0; padding-top: 0.9rem;
    border-top: 1px solid var(--rule-soft); max-width: none;
  }
  .commits b { font-weight: 650; }

  .tip {
    background: var(--paper-2); border: 1px solid var(--rule);
    border-radius: 12px; padding: 1.5rem 1.6rem;
  }
  .tip h3 {
    font-family: var(--serif); font-weight: 600; font-size: 1.2rem;
    margin: 0 0 1rem; color: var(--ink); letter-spacing: -0.008em;
  }
  .tip p { font-family: var(--sans); font-size: 0.96rem; line-height: 1.58; color: var(--ink-2); margin: 0 0 0.9rem; max-width: 64ch; }
  .tip p b { color: var(--ink); font-weight: 650; }
  .tip p em { font-style: italic; }
  .tip-sharp {
    margin-top: 1.1rem !important; padding: 1rem 1.15rem;
    background: var(--card); border-left: 4px solid var(--strain);
    border-radius: 0 8px 8px 0; color: var(--ink) !important; font-size: 0.98rem !important;
  }
  .tip-sharp em { color: var(--strain); font-style: italic; font-weight: 500; }

  /* ---------- colophon ---------- */
  .colophon { border-top: 2px solid var(--ink); margin-top: 3.4rem; padding-top: 1.3rem; }
  .colophon-row {
    display: flex; flex-wrap: wrap; justify-content: space-between; gap: 0.5rem 1.5rem;
    font-family: var(--mono); font-size: 0.72rem; letter-spacing: 0.04em;
    color: var(--slate-dim); text-transform: uppercase; margin-bottom: 0.9rem;
  }
  .sources {
    font-family: var(--sans); font-size: 0.8rem; line-height: 1.7;
    color: var(--slate); max-width: none; margin: 0;
  }
  .sources code { font-size: 0.82em; background: transparent; border: none; padding: 0; color: var(--slate); }

  /* ---------- responsive ---------- */
  @media (max-width: 720px) {
    body { font-size: 16.5px; }
    .two-col, .choice-grid { grid-template-columns: 1fr; }
    .band-head { grid-template-columns: auto 1fr; gap: 0.8rem; }
    .finding { padding-left: 0.9rem; }
  }
  @media (prefers-reduced-motion: reduce) {
    * { animation: none !important; transition: none !important; }
  }
  a:focus-visible, th:focus-visible { outline: 2px solid var(--a); outline-offset: 2px; }
</style>
